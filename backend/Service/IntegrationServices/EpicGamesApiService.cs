// Services/IApiService.cs

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Service.Integration;


// Services/EpicGamesApiService.cs
public class EpicGamesApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public EpicGamesApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _clientId = configuration["EpicGames:ClientId"];
        _clientSecret = configuration["EpicGames:ClientSecret"];
    }

    public async Task<PlatformAccount> GetAccountDataAsync(string accountId, PlatformType platform)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"https://api.epicgames.dev/epic/profile/v1/profiles/{accountId}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EpicProfileResponse>(content);
        
        return new PlatformAccount
        {
            Platform = PlatformType.EpicGames,
            AccountId = accountId,
            AccountName = result.DisplayName
        };
    }

    public async Task<List<Game>> GetOwnedGamesAsync(string accountId, PlatformType platform)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"https://api.epicgames.dev/library/v1/accounts/{accountId}/games");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EpicGamesResponse>(content);
        
        return result.Games.Select(g => new Game
        {
            GameId = g.ProductId,
            Name = g.Title,
            ImageUrl = g.KeyImages.FirstOrDefault(i => i.Type == "Thumbnail")?.Url
        }).ToList();
    }

    public async Task<List<Achievement>> GetAchievementsAsync(string gameId, string accountId, PlatformType platform)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"https://api.epicgames.dev/achievements/v1/products/{gameId}/accounts/{accountId}/achievements");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EpicAchievementsResponse>(content);
        
        return result.Achievements.Select(a => new Achievement
        {
            
            Name = a.UnlockedDisplayName,
            IsAchieved = a.IsUnlocked,
            UnlockTime = a.UnlockTime,
            ImageUrl = a.UnlockedIconUrl
        }).ToList();
    }

    public async Task<List<Statistic>> GetStatisticsAsync(string gameId, string accountId, PlatformType platform)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"https://api.epicgames.dev/stats/v1/products/{gameId}/accounts/{accountId}/stats");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EpicStatsResponse>(content);
        
        return result.Stats.Select(s => new Statistic
        {
            Name = s.Name,
            Value = s.Value.ToString()
        }).ToList();
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.epicgames.dev/auth/v1/oauth/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", _clientId},
                {"client_secret", _clientSecret}
            })
        };
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<EpicTokenResponse>(content);
        
        return tokenResponse.AccessToken;
    }

    public Task<SteamGameInfo> GetGameInfoAsync(string appId)
    {
        throw new NotImplementedException();
    }

    // Response classes for Epic Games API
    private class EpicTokenResponse
    {
        public string AccessToken { get; set; }
    }

    private class EpicProfileResponse
    {
        public string AccountId { get; set; }
        public string DisplayName { get; set; }
    }

    private class EpicGamesResponse
    {
        public List<EpicGame> Games { get; set; }
    }

    private class EpicGame
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public int TotalPlayTime { get; set; }
        public List<EpicKeyImage> KeyImages { get; set; }
    }

    private class EpicKeyImage
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }

    private class EpicAchievementsResponse
    {
        public List<EpicAchievement> Achievements { get; set; }
    }

    private class EpicAchievement
    {
        public string AchievementId { get; set; }
        public string UnlockedDisplayName { get; set; }
        public string UnlockedDescription { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockTime { get; set; }
        public string UnlockedIconUrl { get; set; }
    }

    private class EpicStatsResponse
    {
        public List<EpicStat> Stats { get; set; }
    }

    private class EpicStat
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}