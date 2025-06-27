using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PlatformAccount> PlatformAccounts { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
}