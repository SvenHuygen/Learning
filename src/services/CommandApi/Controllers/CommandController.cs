using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [ApiController]
    [Route("api/c/[controller]s")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        //private readonly ICommandService _commandService;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }
    }
}