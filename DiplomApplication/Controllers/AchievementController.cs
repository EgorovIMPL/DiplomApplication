using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace DiplomApplication.Controllers
{
    [ApiController]
    [Route("api/achievements")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet("game/{gameId}")]
        [Authorize]
        public async Task<IActionResult> GetByGame(int gameId)
        {
            var achievements = await _achievementService.GetAchievementsByGameAsync(gameId);
            return Ok(achievements);
        }

        [HttpPost("sync/{gameId}")]
        [Authorize]
        public async Task<IActionResult> SyncAchievements(int gameId,
            [FromQuery] PlatformType platformType,
            [FromQuery] string accountId)
        {
            await _achievementService.SyncAchievementsAsync(gameId, platformType, accountId);
            return NoContent();
        }
    }
}
