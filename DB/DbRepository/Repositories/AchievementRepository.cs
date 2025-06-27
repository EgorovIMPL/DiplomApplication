using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repositories
{
    public interface IAchievementRepository
    {
        Task<List<Achievement>> GetByGameIdAsync(int gameId);
        Task CreateRangeAsync(IEnumerable<Achievement> achievements);
        Task UpdateAsync(int id, bool isAchieved, DateTime? unlockTime);
    }

    // Repositories/AchievementRepository.cs
    public class AchievementRepository : IAchievementRepository
    {
        private readonly AppDbContext _context;

        public AchievementRepository(AppDbContext context) => _context = context;

        public async Task<List<Achievement>> GetByGameIdAsync(int gameId)
            => await _context.Achievements
                .Where(a => a.GameId == gameId)
                .ToListAsync();

        public async Task CreateRangeAsync(IEnumerable<Achievement> achievements)
        {
            await _context.Achievements.AddRangeAsync(achievements);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, bool isAchieved, DateTime? unlockTime)
        {
            var achieve = await _context.Achievements.FirstOrDefaultAsync(a => a.Id == id);

            if (achieve != null)
            {
                achieve.UnlockTime = unlockTime;
                achieve.IsAchieved = isAchieved;

                _context.Achievements.Update(achieve);
                await _context.SaveChangesAsync();
            }
        }
    }
}
