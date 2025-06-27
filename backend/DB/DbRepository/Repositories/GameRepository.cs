using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DbRepository.Repositories
{
    // Repositories/IGameRepository.cs
    public interface IGameRepository
    {
        Task<Game> GetByIdAsync(int id);
        Task<List<Game>> GetByPlatformAccountIdAsync(int platformAccountId);
        Task<bool> ExistsAsync(int platformAccountId, string gameId);
        Task CreateAsync(Game game);
        Task CreateRangeAsync(IEnumerable<Game> games);
        int GetCount();
    }

    // Repositories/GameRepository.cs
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context) => _context = context;

        public async Task<Game> GetByIdAsync(int id)
            => await _context.Games.FindAsync(id);

        public async Task<List<Game>> GetByPlatformAccountIdAsync(int platformAccountId)
            => await _context.Games
                .Where(g => g.PlatformAccountId == platformAccountId)
                .ToListAsync();

        public async Task<bool> ExistsAsync(int platformAccountId, string gameId)
            => await _context.Games
                .AnyAsync(g => g.PlatformAccountId == platformAccountId && g.GameId == gameId);

        public async Task CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Game> games)
        {
            await _context.Games.AddRangeAsync(games);
            await _context.SaveChangesAsync();
        }

        public int GetCount()
            => _context.Games.Count();
    }
}
