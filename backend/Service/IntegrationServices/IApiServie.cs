using Infrastructure.Models;

namespace Service.Integration;

public interface IApiService
{
    Task<PlatformAccount> GetAccountDataAsync(string accountId, PlatformType platform);
    Task<List<Game>> GetOwnedGamesAsync(string accountId, PlatformType platform);
    Task<List<Achievement>> GetAchievementsAsync(string gameId, string accountId, PlatformType platform);
    Task<List<Statistic>> GetStatisticsAsync(string gameId, string accountId, PlatformType platform);
    Task<SteamGameInfo> GetGameInfoAsync(string appId);
}