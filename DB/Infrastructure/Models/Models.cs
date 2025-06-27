namespace Infrastructure.Models;


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

public class User : IdentityUser
{
    public string DisplayName { get; set; }
}


public class PlatformAccount
{
    public int Id { get; set; }
    public PlatformType Platform { get; set; }
    public string AccountId { get; set; }
    public string AccountName { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}

public class Game
{
    public int Id { get; set; }
    public string GameId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PlatformAccountId { get; set; }
    public PlatformAccount PlatformAccount { get; set; }
}

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string ImageUrl { get; set; }
    public bool IsAchieved { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UnlockTime { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}

public class Statistic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}