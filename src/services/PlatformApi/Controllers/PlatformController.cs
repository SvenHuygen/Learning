using Microsoft.AspNetCore.Mvc;
using PlatformApi.Business.Abstractions;
using PlatformApi.Models.Dto;

namespace PlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class PlatformController : ControllerBase
    {
        private readonly ILogger<PlatformController> _logger;
        private readonly IPlatformService _platformService;

        public PlatformController(ILogger<PlatformController> logger, IPlatformService platformService)
        {
            _logger = logger;
            _platformService = platformService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlatformReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get All Platforms Called.");
            var all =  await _platformService.GetAll();
            if (all == null) return NotFound();
            return Ok(all);
        }

        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(typeof(PlatformReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Get PlatformById Called with ID: {id}.");
            var found = await _platformService.GetById(id);
            if(found == null) return NotFound();
            return Ok(found);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlatformReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(PlatformCreateDto dto)
        {
            _logger.LogInformation($"Get PlatformById Called with parmater: {dto.Name} -- {dto.Publisher} -- {dto.Cost}.");
            var created = await _platformService.Create(dto);
            if(created == null) return NotFound();
            return CreatedAtRoute("GetById", new {id = created.Id} ,created);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAll(){
                 _logger.LogInformation("Delete All Called.");
            var response = await _platformService.DeleteAll();
            if(response){
                return NoContent();
            } else {
                return BadRequest("Something went wrong while deleting all platforms.");
            }
        }
    }
}