using CommandApi.Business.Abstractions;
using CommandApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [ApiController]
    [Route("api/c/[controller]s")]
    public class PlatformController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        private readonly ICommandService _commandService;

        public PlatformController(ILogger<CommandController> logger, ICommandService commandService)
        {
            _logger = logger;
            _commandService = commandService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlatformReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlatforms()
        {
            var result = await _commandService.GetAllPlatforms();
            if(result == null) return NotFound();

            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult TestInboundConnection()
        {
            _logger.LogInformation("--> Inboud POST # Command Service");

            return Ok("POST Test Success!");
        }
    }
}
