using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repositories
{
    public interface IStatisticRepository
    {
        Task<List<Statistic>> GetByGameIdAsync(int gameId);
        Task CreateRangeAsync(IEnumerable<Statistic> statistics);
    }

    // Repositories/StatisticRepository.cs
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AppDbContext _context;

        public StatisticRepository(AppDbContext context) => _context = context;

        public async Task<List<Statistic>> GetByGameIdAsync(int gameId)
            => await _context.Statistics
                .Where(s => s.GameId == gameId)
                .ToListAsync();

        public async Task CreateRangeAsync(IEnumerable<Statistic> statistics)
        {
            await _context.Statistics.AddRangeAsync(statistics);
            await _context.SaveChangesAsync();
        }
    }
}
