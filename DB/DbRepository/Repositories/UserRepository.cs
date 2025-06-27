using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DbRepository.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(string id);
    Task CreateAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(string id)
        => await _context.Users.FindAsync(id);

    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}