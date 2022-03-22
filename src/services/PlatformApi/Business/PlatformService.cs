using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformApi.Business.Abstractions;
using PlatformApi.Data;
using PlatformApi.HttpDataServices.Abstractions;
using PlatformApi.Models;
using PlatformApi.Models.Dto;

namespace PlatformApi.Business
{
    public class PlatformService : IPlatformService
    {
        private readonly PlatformDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _client;

        public PlatformService(PlatformDbContext context, IMapper mapper, ICommandDataClient client)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
        }

        public async Task<PlatformReadDto> Create(PlatformCreateDto dto)
        {
            var newPlatform = _mapper.Map<Platform>(dto);
            newPlatform.Id = Guid.NewGuid();
            await _context.AddAsync(newPlatform);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<PlatformReadDto>(
                        await _context.Platforms.FirstOrDefaultAsync(x => x.Id == newPlatform.Id)
                    );
            try
            {
                await _client.SendPlatformToCommand(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<IEnumerable<PlatformReadDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<PlatformReadDto>>(
                    await _context.Platforms.ToListAsync()
                );
        }

        public async Task<PlatformReadDto> GetById(Guid id)
        {
            return _mapper.Map<PlatformReadDto>(
                    await _context.Platforms.FirstOrDefaultAsync(x => x.Id == id)
                );
        }
    }
}
