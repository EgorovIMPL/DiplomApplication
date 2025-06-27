using DbRepository.Repositories;
using DiplomApplication;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IAchievementService
    {
        Task<List<AchievementDto>> GetAchievementsByGameAsync(int gameId);
        Task SyncAchievementsAsync(int gameId, PlatformType platformType, string accountId);
    }

    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IApiServiceFactory _apiServiceFactory;

        public AchievementService(
            IAchievementRepository achievementRepository,
            IGameRepository gameRepository,
            IApiServiceFactory apiServiceFactory)
        {
            _achievementRepository = achievementRepository;
            _gameRepository = gameRepository;
            _apiServiceFactory = apiServiceFactory;
        }

        public async Task<List<AchievementDto>> GetAchievementsByGameAsync(int gameId)
        {
            var achievements = await _achievementRepository.GetByGameIdAsync(gameId);
            return achievements.Select(a => a.ToDto()).ToList();
        }

        public async Task SyncAchievementsAsync(int gameId, PlatformType platformType, string accountId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null) throw new ArgumentException("Game not found");

            var apiService = _apiServiceFactory.Create(platformType);
            var achievements = await apiService.GetAchievementsAsync(game.GameId, accountId, platformType);

            var gameAchievementIds = (await _achievementRepository.GetByGameIdAsync(gameId))
                .Select(a => a.Id)
                .ToList();

            for(var i = 0; i < gameAchievementIds.Count; i++) 
            {
                if (achievements[i].IsAchieved)
                {
                    await _achievementRepository.UpdateAsync(gameAchievementIds[i], true, achievements[i].UnlockTime);
                }
            }
        }
    }
}
