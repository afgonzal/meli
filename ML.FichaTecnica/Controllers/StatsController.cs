using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.Services;

namespace ML.FichaTecnica.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        private readonly ILogger<StatsController> _logger;

        public StatsController(IStatsService statsService, ILogger<StatsController> logger)
        {
            _statsService = statsService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            _logger.LogInformation("Get stats by id:{id}.", id);
            try
            {
                var result = await _statsService.ExtractStatsById(id);
                _logger.LogInformation("Get stats by id:{id} finished.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected error getting stats by id:{id}.";
                _logger.LogError(msg, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
        [HttpGet("dateRange")]
        public async Task<IActionResult> GetById([FromQuery] DateTimeOffset dateFrom,[FromQuery]DateTimeOffset dateTo)
        {
            _logger.LogInformation("Get stats by date range:{dateFrom}-{dateTo}.", dateFrom, dateTo);
            try
            {
                var result = await _statsService.ExtractStatsByDateRange(dateFrom, dateTo);
                _logger.LogInformation("Get stats by date range:{dateFrom}-{dateTo} finished.", dateFrom, dateTo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected error getting stats by date range:{dateFrom} - {dateTo}.";
                _logger.LogError(msg, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}
