using CommandApi.Business.Abstractions;
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
        public async Task<IActionResult> GetPlatforms()
        {
            var result = await _commandService.GetAllPlatforms();
            if(result == null) return NotFound();

            return Ok(result);
        }


        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inboud POST # Command Service");

            return Ok("POST Test Success!");
        }
    }
}
