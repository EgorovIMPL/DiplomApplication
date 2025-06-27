using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace DiplomApplication.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("platform/{platformAccountId}")]
        [Authorize]
        public async Task<IActionResult> GetByPlatform(int platformAccountId)
        {
            var games = await _gameService.GetGamesByPlatformAsync(platformAccountId);
            return Ok(games);
        }

        [HttpPost("sync/{platformAccountId}")]
        [Authorize]
        public async Task<IActionResult> SyncGames(int platformAccountId,
            [FromQuery] PlatformType platformType,
            [FromQuery] string accountId)
        {
            await _gameService.SyncGamesAsync(platformAccountId, platformType, accountId);
            return NoContent();
        }
    }
}
