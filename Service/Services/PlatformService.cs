using DbRepository.Repositories;
using DiplomApplication;
using Infrastructure.Models;

namespace Service.Services;

// Services/IPlatformService.cs
public interface IPlatformService
{
    Task<List<PlatformAccountDto>> GetUserAccountsAsync(string userId);
    Task AddAccountAsync(string userId, CreatePlatformAccountDto accountDto);
    Task RemoveAccountAsync(int accountId);
}

// Services/PlatformService.cs
public class PlatformService : IPlatformService
{
    private readonly IPlatformAccountRepository _accountRepository;
    private readonly IApiServiceFactory _apiServiceFactory;
    private readonly IUserRepository _userRepository;

    public PlatformService(
        IPlatformAccountRepository accountRepository,
        IApiServiceFactory apiServiceFactory, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _apiServiceFactory = apiServiceFactory;
        _userRepository = userRepository;
    }

    public async Task<List<PlatformAccountDto>> GetUserAccountsAsync(string userId)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(userId);
        return accounts.Select(a => a.ToDto()).ToList();
    }

    public async Task AddAccountAsync(string userId, CreatePlatformAccountDto accountDto)
    {
        if (await _accountRepository.ExistsAsync(userId, accountDto.Platform, accountDto.AccountId))
        {
            throw new InvalidOperationException("Account already exists");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        var apiService = _apiServiceFactory.Create(accountDto.Platform);
        var accountData = await apiService.GetAccountDataAsync(accountDto.AccountId, accountDto.Platform);
        
        var account = new PlatformAccount
        {
            Platform = accountDto.Platform,
            AccountId = accountDto.AccountId,
            AccountName = accountData.AccountName,
            User = user
        };

        await _accountRepository.CreateAsync(account);
    }

    public async Task RemoveAccountAsync(int accountId)
    {
        await _accountRepository.DeleteAsync(accountId);
    }
}