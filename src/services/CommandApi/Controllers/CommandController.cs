using CommandApi.Business.Abstractions;
using CommandApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CommandApi.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]s")]
    public class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;
        private readonly ICommandService _commandService;

        public CommandController(ILogger<CommandController> logger, ICommandService commandService)
        {
            _logger = logger;
            _commandService = commandService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CommandReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommandsForPlatform(Guid platformId){
            var result = await _commandService.GetCommandsForPlatform(platformId);
            if(result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        [ProducesResponseType(typeof(CommandReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommandForPlatform(Guid platformId, Guid commandId){
            var result = await _commandService.GetCommand(platformId, commandId);

            if(result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCommandForPlatform(Guid platformId, CommandCreateDto dto){
            var found = await _commandService.GetPlatformById(platformId);
            if(found == null) return NotFound("Platform not found.");

            var command = await _commandService.CreateCommand(platformId, dto);

            return CreatedAtRoute("GetCommandForPlatform", new {platformId = command.PlatformId, commandApi = command.Id} ,command);
        }
    }
}