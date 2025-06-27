using Infrastructure.Models;

namespace Service;

public static class MappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            DisplayName = user.DisplayName
        };
    }

    public static PlatformAccountDto ToDto(this PlatformAccount account)
    {
        return new PlatformAccountDto
        {
            Platform = account.Platform,
            AccountId = account.AccountId,
            Name = account.AccountName,
            UserId = account.UserId
        };
    }

    public static GameDto ToDto(this Game game)
    {
        return new GameDto
        {
            GameId = game.GameId,
            Name = game.Name,
            ImageUrl = game.ImageUrl,
            PlatformId = game.PlatformAccountId
        };
    }

    public static AchievementDto ToDto(this Achievement achievement)
    {
        return new AchievementDto
        {
            Name = achievement.Name,
            IsAchieved = achievement.IsAchieved,
            UnlockTime = achievement.UnlockTime,
            ImageUrl = achievement.ImageUrl,
            GameId = achievement.GameId
        };
    }

    public static StatisticDto ToDto(this Statistic statistic)
    {
        return new StatisticDto
        {
            Name = statistic.Name,
            Value = statistic.Value,
            GameId = statistic.GameId
        };
    }
}