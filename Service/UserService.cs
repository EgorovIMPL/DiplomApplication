using Contract;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class UserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task Create(CreateUserDto userDto)
    {
        if (userDto.SteamId == 0)
        {
            return;
        }
        
        var user = new User()
        {
            UserName = userDto.Name,
            Email = userDto.Email,
            P
        }

        _databaseContext.Users.Add(user);
    }
}