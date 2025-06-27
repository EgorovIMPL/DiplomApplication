/*using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DiplomApplication;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly IApiServiceFactory _apiServiceFactory;

    public AccountController(
        UserManager<User> userManager,
        AppDbContext context,
        IApiServiceFactory apiServiceFactory)
    {
        _userManager = userManager;
        _context = context;
        _apiServiceFactory = apiServiceFactory;
    }

    [HttpGet("platforms")]
    [Authorize]
    public async Task<IActionResult> GetPlatformAccounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var accounts = await _context.PlatformAccounts
            .Where(pa => pa.User.Id == userId)
            .ToListAsync();

        return Ok(accounts);
    }

    [HttpPost("platforms")]
    [Authorize]
    public async Task<IActionResult> AddPlatformAccount([FromBody] AddPlatformAccountRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        // Проверяем, есть ли уже такая платформа у пользователя
        var existingAccount = await _context.PlatformAccounts
            .FirstOrDefaultAsync(pa =>
                pa.User.Id == userId &&
                pa.Platform == request.Platform &&
                pa.AccountId == request.AccountId);

        if (existingAccount != null)
        {
            return Conflict("This platform account is already linked to your profile");
        }

        var apiService = _apiServiceFactory.Create(request.Platform);
        var accountData = await apiService.GetAccountDataAsync(request.AccountId, request.Platform);

        // Устанавливаем связь с пользователем
        accountData.User = user;

        _context.PlatformAccounts.Add(accountData);
        await _context.SaveChangesAsync();

        return Ok(accountData);
    }

    [HttpDelete("platforms/{id}")]
    [Authorize]
    public async Task<IActionResult> RemovePlatformAccount(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var account = await _context.PlatformAccounts
            .FirstOrDefaultAsync(pa => pa.Id == id && pa.User.Id == userId);

        if (account == null) return NotFound();

        _context.PlatformAccounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("platforms/{id}/games")]
    [Authorize]
    public async Task<IActionResult> GetGames(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var account = await _context.PlatformAccounts
            .Include(pa => pa.User)
            .FirstOrDefaultAsync(pa => pa.Id == id && pa.User.Id == userId);

        if (account == null) return NotFound();

        var apiService = _apiServiceFactory.Create(account.Platform);
        var games = await apiService.GetOwnedGamesAsync(account.AccountId, account.Platform);

        foreach (var game in games)
        {
            var gameInfo = await apiService.GetGameInfoAsync(game.GameId);

        }

        // Получаем существующие игры для этого аккаунта
        var existingGameIds = await _context.Games
            .Where(g => g.PlatformAccountId == account.Id)
            .Select(g => g.GameId)
            .ToListAsync();

        // Сохраняем только новые игры
        var newGames = games.Where(g => !existingGameIds.Contains(g.GameId)).ToList();

        foreach (var game in newGames)
        {
            game.PlatformAccount = account;
            game.PlatformAccountId = account.Id;
            _context.Games.Add(game);
        }

        await _context.SaveChangesAsync();

        return Ok(newGames);
    }

    [HttpGet("games/{id}/achievements")]
    [Authorize]
    public async Task<IActionResult> GetAchievements(int id)
    {
        var game = await _context.Games
            .Include(g => g.PlatformAccount)
            .ThenInclude(pa => pa.User)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (game.PlatformAccount.User.Id != userId) return Forbid();

        var apiService = _apiServiceFactory.Create(game.PlatformAccount.Platform);
        var achievements = await apiService.GetAchievementsAsync(
            game.GameId,
            game.PlatformAccount.AccountId,
            game.PlatformAccount.Platform);

        // Получаем существующие достижения для этой игры
        var existingAchievementIds = await _context.Achievements
            .Where(a => a.GameId == game.Id)
            .Select(a => a.Name)
            .ToListAsync();

        // Сохраняем только новые достижения
        var newAchievements = achievements
            .Where(a => !existingAchievementIds.Contains(a.Name))
            .ToList();

        foreach (var achievement in newAchievements)
        {
            achievement.Game = game;
            achievement.GameId = game.Id;
            _context.Achievements.Add(achievement);
        }

        await _context.SaveChangesAsync();

        return Ok(newAchievements);
    }

    [HttpGet("games/{id}/statistics")]
    [Authorize]
    public async Task<IActionResult> GetStatistics(int id)
    {
        var game = await _context.Games
            .Include(g => g.PlatformAccount)
            .ThenInclude(pa => pa.User)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (game.PlatformAccount.User.Id != userId) return Forbid();

        var apiService = _apiServiceFactory.Create(game.PlatformAccount.Platform);
        var statistics = await apiService.GetStatisticsAsync(
            game.GameId,
            game.PlatformAccount.AccountId,
            game.PlatformAccount.Platform);

        // Получаем существующую статистику для этой игры
        var existingStatNames = await _context.Statistics
            .Where(s => s.GameId == game.Id)
            .Select(s => s.Name)
            .ToListAsync();

        // Сохраняем только новую статистику
        var newStatistics = statistics
            .Where(s => !existingStatNames.Contains(s.Name))
            .ToList();

        foreach (var stat in newStatistics)
        {
            stat.Game = game;
            stat.GameId = game.Id;
            _context.Statistics.Add(stat);
        }

        await _context.SaveChangesAsync();

        return Ok(newStatistics);
    }
}*/