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
    public interface IGameService
    {
        Task<List<GameDto>> GetGamesByPlatformAsync(int platformAccountId);
        Task SyncGamesAsync(int platformAccountId, PlatformType platformType, string accountId);
    }

    // Services/GameService.cs
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IApiServiceFactory _apiServiceFactory;
        private readonly IAchievementRepository _achievementRepository;

        public GameService(IGameRepository gameRepository, IApiServiceFactory apiServiceFactory, IAchievementRepository achievementRepository)
        {
            _gameRepository = gameRepository;
            _apiServiceFactory = apiServiceFactory;
            _achievementRepository = achievementRepository;
        }

        public async Task<List<GameDto>> GetGamesByPlatformAsync(int platformAccountId)
        {
            var games = await _gameRepository.GetByPlatformAccountIdAsync(platformAccountId);
            return games.Select(g => g.ToDto()).ToList();
        }

        public async Task SyncGamesAsync(int platformAccountId, PlatformType platformType, string accountId)
        {
            var apiService = _apiServiceFactory.Create(platformType);
            var games = await apiService.GetOwnedGamesAsync(accountId, platformType);

            var existingGameIds = (await _gameRepository.GetByPlatformAccountIdAsync(platformAccountId))
                .Select(g => g.GameId)
                .ToList();

            var newGames = games
                .Where(g => !existingGameIds.Contains(g.GameId))
                .Select(g => new Game
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    PlatformAccountId = platformAccountId
                });

            foreach (var game in newGames)
            {
                var gameInfo = await apiService.GetGameInfoAsync(game.GameId);
                var gameId = _gameRepository.GetCount() + 1;

                await _gameRepository.CreateAsync(new Game
                {
                    Id = gameId,
                    GameId = game.GameId,
                    Name = game.Name,
                    Description = gameInfo.ShortDescription,
                    ImageUrl = gameInfo.HeaderImage,
                    PlatformAccountId = platformAccountId
                });

                _achievementRepository.CreateRangeAsync(gameInfo.Achievements.Select(a => new Achievement
                {
                    Name = a.Name,
                    ImageUrl = a.IconUrl,
                    GameId = gameId
                }));
            }
        }
    }
}
