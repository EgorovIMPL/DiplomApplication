using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Services;

namespace DiplomApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public AccountController(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet("platforms")]
    [Authorize]
    public async Task<IActionResult> GetPlatformAccounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var accounts = await _platformService.GetUserAccountsAsync(userId);
        return Ok(accounts);
    }

    [HttpPost("platforms")]
    [Authorize]
    public async Task<IActionResult> AddPlatformAccount([FromBody] CreatePlatformAccountDto accountDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _platformService.AddAccountAsync(userId, accountDto);
        return Ok();
    }

    [HttpDelete("platforms/{id}")]
    [Authorize]
    public async Task<IActionResult> RemovePlatformAccount(int id)
    {
        await _platformService.RemoveAccountAsync(id);
        return NoContent();
    }
}