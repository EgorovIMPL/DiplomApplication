using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DbRepository.Repositories;

// Repositories/IPlatformAccountRepository.cs
public interface IPlatformAccountRepository
{
    Task<PlatformAccount> GetByIdAsync(int id);
    Task<List<PlatformAccount>> GetByUserIdAsync(string userId);
    Task<bool> ExistsAsync(string userId, PlatformType platform, string accountId);
    Task CreateAsync(PlatformAccount account);
    Task DeleteAsync(int id);
}

// Repositories/PlatformAccountRepository.cs
public class PlatformAccountRepository : IPlatformAccountRepository
{
    private readonly AppDbContext _context;

    public PlatformAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlatformAccount> GetByIdAsync(int id)
        => await _context.PlatformAccounts.FindAsync(id);

    public async Task<List<PlatformAccount>> GetByUserIdAsync(string userId)
        => await _context.PlatformAccounts
            .Where(pa => pa.UserId == userId)
            .ToListAsync();

    public async Task<bool> ExistsAsync(string userId, PlatformType platform, string accountId)
        => await _context.PlatformAccounts
            .AnyAsync(pa =>
                pa.UserId == userId &&
                (int)pa.Platform == (int)platform &&
                pa.AccountId == accountId);

    public async Task CreateAsync(PlatformAccount account)
    {
        await _context.PlatformAccounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _context.PlatformAccounts.FindAsync(id);
        if (account != null)
        {
            _context.PlatformAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}