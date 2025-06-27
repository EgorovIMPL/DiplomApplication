using DbRepository.Repositories;
using Infrastructure.Models;

namespace Service.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(string id);
    Task CreateUserAsync(UserDto userDto);
}


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.ToDto();
    }

    public async Task CreateUserAsync(UserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            DisplayName = userDto.DisplayName
        };
        
        await _userRepository.CreateAsync(user);
    }
}