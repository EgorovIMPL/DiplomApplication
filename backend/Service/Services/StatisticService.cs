using DbRepository.Repositories;
using DiplomApplication;

namespace Service.Services
{
    public interface IStatisticService
    {
        Task<List<StatisticDto>> GetStatisticsByGameAsync(int gameId);
        Task<bool> SyncStatisticsAsync(int gameId, PlatformType platformType, string accountId);
    }

    // Services/StatisticService.cs
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IApiServiceFactory _apiServiceFactory;
        public StatisticService(
        IStatisticRepository statisticRepository,
        IGameRepository gameRepository,
        IApiServiceFactory apiServiceFactory)
        {
            _statisticRepository = statisticRepository;
            _gameRepository = gameRepository;
            _apiServiceFactory = apiServiceFactory;
        }

        public async Task<List<StatisticDto>> GetStatisticsByGameAsync(int gameId)
        {
            var statistics = await _statisticRepository.GetByGameIdAsync(gameId);
            return statistics.Select(s => s.ToDto()).ToList();
        }

        public async Task<bool> SyncStatisticsAsync(int gameId, PlatformType platformType, string accountId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null) throw new ArgumentException("Game not found");

            var apiService = _apiServiceFactory.Create(platformType);
            var statistics = await apiService.GetStatisticsAsync(game.GameId, accountId, platformType);

            if (statistics != null)
            {
                statistics.ForEach(s => s.GameId = gameId);

                await _statisticRepository.CreateRangeAsync(statistics);

                return true;
            }

            return false;
        }
    }
}
