using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;

public class User: IdentityUser
{
    public long SteamId;
    public Guid EpicId;
    //todo надо будет проверить дополнительно
}