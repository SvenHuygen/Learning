using AutoMapper;
using CommandApi.Business.Abstractions;
using CommandApi.Data;
using CommandApi.Models;
using CommandApi.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace CommandApi.Business
{
    public class CommandService : ICommandService
    {
        private readonly CommandDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandService> _logger;

        public CommandService(CommandDbContext context, IMapper mapper, ILogger<CommandService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommandReadDto> CreateCommand(Guid platformId, CommandCreateDto dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto));
            
            var newCommand = _mapper.Map<Command>(dto);
            newCommand.Id = Guid.NewGuid();
            newCommand.PlatformId = platformId;
            
            await _context.Commands.AddAsync(newCommand);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommandReadDto>(newCommand);
        }

        public async Task<PlatformReadDto> CreatePlatform(PlatformCreateDto dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto));
          
            await _context.Platforms.AddAsync(_mapper.Map<Platform>(dto));
            await _context.SaveChangesAsync();

            return _mapper.Map<PlatformReadDto>(dto);
        }

        public async Task<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            return _mapper.Map<List<PlatformReadDto>>(
                await _context.Platforms.ToListAsync()
            );
        }

        public async Task<CommandReadDto> GetCommand(Guid platformId, Guid commandId)
        {
            return _mapper.Map<CommandReadDto>(
                await _context.Commands.Where(x => x.Id == commandId && x.PlatformId == platformId).FirstOrDefaultAsync()
            );
        }

        public async Task<IEnumerable<CommandReadDto>> GetCommandsForPlatform(Guid platformId)
        {
            var found = await GetPlatformById(platformId);
            if(found == null) return null;

            _logger.LogInformation($"Commands: {await _context.Commands.Where(c => c.PlatformId == platformId).FirstOrDefaultAsync()}");

            return _mapper.Map<List<CommandReadDto>>(
                await _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ToListAsync()
            );
        }

        public async Task<PlatformReadDto> GetExternalPlatformById(Guid platformId)
        {
             return _mapper.Map<PlatformReadDto>(
                await _context.Platforms.FirstOrDefaultAsync(x => x.ExternalId == platformId)
            );
        }

        public async Task<PlatformReadDto> GetPlatformById(Guid platformId)
        {
            return _mapper.Map<PlatformReadDto>(
                await _context.Platforms.FirstOrDefaultAsync(x => x.Id == platformId)
            );
        }
    }
}