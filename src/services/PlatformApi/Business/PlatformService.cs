using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformApi.Business.Abstractions;
using PlatformApi.Data;
using PlatformApi.Models;
using PlatformApi.Models.Dto;

namespace PlatformApi.Business
{
    public class PlatformService : IPlatformService
    {
        private readonly PlatformDbContext _context;
        private readonly IMapper _mapper;

        public PlatformService(PlatformDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlatformReadDto> Create(PlatformCreateDto dto)
        {
            var newPlatform = _mapper.Map<Platform>(dto);
            newPlatform.Id = Guid.NewGuid();          
            await _context.AddAsync(newPlatform);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlatformReadDto>(
                        await _context.Platforms.FirstOrDefaultAsync(x => x.Id == newPlatform.Id)
                    );
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
