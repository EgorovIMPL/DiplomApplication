public class UserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
}

// DTOs/PlatformAccountDto.cs
public class PlatformAccountDto
{
    public PlatformType Platform { get; set; }
    public string Name { get; set; }
    public string AccountId { get; set; }
    public string UserId { get; set; }
}

public class CreatePlatformAccountDto
{
    public PlatformType Platform { get; set; }
    public string AccountId { get; set; }
}

// DTOs/GameDto.cs
public class GameDto
{
    public string GameId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int PlatformId { get; set; }
}

// DTOs/AchievementDto.cs
public class AchievementDto
{
    public string Name { get; set; }
    public bool IsAchieved { get; set; }
    public DateTime? UnlockTime { get; set; }
    public string ImageUrl { get; set; }
    public int GameId { get; set; }
}

// DTOs/StatisticDto.cs
public class StatisticDto
{
    public string Name { get; set; }
    public string Value { get; set; }
    public int GameId { get; set; }
}

public enum PlatformType
{
    Steam,
    EpicGames
}