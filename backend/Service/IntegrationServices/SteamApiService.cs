using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using WebApiResponses;

namespace Service.Integration;


public class SteamGameInfo
{
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string HeaderImage { get; set; } = string.Empty;
    public List<GameAchievement> Achievements { get; set; } = new();
}

public class GameAchievement
{
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
}

// Services/SteamApiService.cs
public class SteamApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public SteamApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Steam:ApiKey"];
    }

    public async Task<PlatformAccount> GetAccountDataAsync(string accountId, PlatformType platform)
    {
        var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey}&steamids={accountId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SteamPlayerSummaryResponse>(content, _options);
        
        return new PlatformAccount
        {
            Platform = PlatformType.Steam,
            AccountId = accountId,
            AccountName = result.Response.Players.First().PersonaName
        };
    }

    public async Task<List<Game>> GetOwnedGamesAsync(string accountId, PlatformType platform)
    {
        var response = await _httpClient.GetAsync($"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={_apiKey}&steamid={accountId}&include_appinfo=true&include_played_free_games=true");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SteamOwnedGamesResponse>(content, _options);

        return result.Response.Games.Select(g => new Game
        {
            GameId = g.AppId.ToString(),
            Name = g.Name,
        }).ToList();
    }

    public async Task<SteamGameInfo> GetGameInfoAsync(string appId)
    {
        var response = await _httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get game info. Status code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Steam возвращает ответ в формате { "appid": { данные } }
        var responseDict = JsonSerializer.Deserialize<Dictionary<string, SteamGameResponse>>(content, options);

        if (responseDict == null || !responseDict.TryGetValue(appId.ToString(), out var gameResponse) || !gameResponse.Success)
        {
            throw new Exception("Failed to parse Steam API response");
        }

        return new SteamGameInfo
        {
            Name = gameResponse.Data?.Name ?? "",
            ShortDescription = gameResponse.Data?.ShortDescription ?? "",
            HeaderImage = gameResponse.Data?.HeaderImage ?? "",
            Achievements = gameResponse.Data?.Achievements?.Highlighted?
                .Select(a => new GameAchievement
                {
                    Name = a.Name ?? "Unknown",
                    IconUrl = a.Path ?? string.Empty
                }).ToList() ?? new List<GameAchievement>()
        };
    }

    public async Task<List<Achievement>> GetAchievementsAsync(string gameId, string accountId, PlatformType platform)
    {
        var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001/?appid={gameId}&key={_apiKey}&steamid={accountId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SteamPlayerAchievementsResponse>(content, _options);
        
        return result.PlayerStats.Achievements.Select(a => new Achievement
        {
            IsAchieved = a.Achieved == 1,
            UnlockTime = DateTimeOffset.FromUnixTimeSeconds(a.UnlockTime).DateTime
        }).ToList();
    }

    public async Task<List<Statistic>> GetStatisticsAsync(string gameId, string accountId, PlatformType platform)
    {
        var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUserStats/GetUserStatsForGame/v0002/?appid={gameId}&key={_apiKey}&steamid={accountId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SteamUserStatsForGameResponse>(content, _options);
        
        return result.PlayerStats.Stats!=null ? result.PlayerStats.Stats.Select(s => new Statistic
        {
            Name = s.Name,
            Value = s.Value.ToString()
        }).ToList() : null;
    }

    // Response classes for Steam API
    private class SteamPlayerSummaryResponse
    {
        public SteamPlayerSummaryContainer Response { get; set; }
    }

    private class SteamPlayerSummaryContainer
    {
        public List<SteamPlayer> Players { get; set; }
    }

    private class SteamPlayer
    {
        [JsonPropertyName("steamid")]
        public string SteamId { get; set; }

        [JsonPropertyName("communityvisibilitystate")]
        public int CommunityVisibilityState { get; set; }

        [JsonPropertyName("profilestate")]
        public int ProfileState { get; set; }

        [JsonPropertyName("personaname")]
        public string PersonaName { get; set; }

        [JsonPropertyName("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonPropertyName("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonPropertyName("avatarhash")]
        public string AvatarHash { get; set; }

        [JsonPropertyName("lastlogoff")]
        public long LastLogoff { get; set; }

        [JsonPropertyName("personastate")]
        public int PersonaState { get; set; }

        [JsonPropertyName("realname")]
        public string RealName { get; set; }

        [JsonPropertyName("primaryclanid")]
        public string PrimaryClanId { get; set; }

        [JsonPropertyName("timecreated")]
        public long TimeCreated { get; set; }

        [JsonPropertyName("personastateflags")]
        public int PersonaStateFlags { get; set; }
    }

    private class SteamOwnedGamesResponse
    {
        public SteamOwnedGamesContainer Response { get; set; }
    }

    private class SteamOwnedGamesContainer
    {
        public List<SteamGame> Games { get; set; }
    }

    private class SteamGame
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public int PlaytimeForever { get; set; }
        public string ImgIconUrl { get; set; }
    }

    private class SteamPlayerAchievementsResponse
    {
        public SteamPlayerAchievements PlayerStats { get; set; }
    }

    private class SteamPlayerAchievements
    {
        public List<SteamAchievement> Achievements { get; set; }
    }

    private class SteamAchievement
    {
        public string ApiName { get; set; }
        public string Description { get; set; }
        public int Achieved { get; set; }
        public long UnlockTime { get; set; }
    }

    private class SteamUserStatsForGameResponse
    {
        public SteamUserStats PlayerStats { get; set; }
    }

    private class SteamUserStats
    {
        public List<SteamStat> Stats { get; set; }
    }

    private class SteamStat
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}