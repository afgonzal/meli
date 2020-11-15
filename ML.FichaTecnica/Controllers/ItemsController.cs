using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.Services;
using Newtonsoft.Json;

namespace ML.FichaTecnica.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IFichaTecnicaService _fichaTecnicaService;
        private readonly IStatsService _statsService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IFichaTecnicaService fichaTecnicaService, IStatsService statsService, ILogger<ItemsController> logger)
        {
            _fichaTecnicaService = fichaTecnicaService;
            _statsService = statsService;
            _logger = logger;
        }
     

        [HttpGet("{id}/attributes")]
        public async Task<IActionResult> GetAttributes([FromRoute] string id)
        {
            _logger.LogInformation("Get item {id} attributes.", id);
            try
            {
                var result = await _fichaTecnicaService.BuildItemAttributes(id);
                _logger.LogTrace("Get item {id} attributes response: {@result}", id, result);
                _logger.LogInformation("Get item {id} attributes finished.", id);
                _statsService.RecordEvent("Items.GetAttributes", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Unexpected error getting Item {id} attributes.";
                _logger.LogError(msg, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}
