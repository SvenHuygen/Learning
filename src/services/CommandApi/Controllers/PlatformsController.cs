using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [ApiController]
    [Route("api/c/[controller]s")]
    public class PlatformController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        //private readonly ICommandService _commandService;

        public PlatformController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inboud POST # Command Service");

            return Ok("POST Test Success!");
        }
    }
}
