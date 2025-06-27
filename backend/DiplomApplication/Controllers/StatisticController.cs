using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace DiplomApplication.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("game/{gameId}")]
        [Authorize]
        public async Task<IActionResult> GetByGame(int gameId)
        {
            var statistics = await _statisticService.GetStatisticsByGameAsync(gameId);
            return Ok(statistics);
        }

        [HttpPost("sync/{gameId}")]
        [Authorize]
        public async Task<IActionResult> SyncStatistics(int gameId,
        [FromQuery] PlatformType platformType,
        [FromQuery] string accountId)
        {
            var result = await _statisticService.SyncStatisticsAsync(gameId, platformType, accountId);
            return result ? Ok("Statistic received") : NotFound("Statistic for this game not supported");
        }
    }
}
