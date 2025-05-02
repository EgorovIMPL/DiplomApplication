namespace Infrastructure.Models;

public class UserGameStat
{
    public long Id;
    public long UserId;
    public long GameId;
    public uint MinutesInGame;
    public DateTime LastGameSection;
}