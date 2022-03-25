using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformApi.Business.Abstractions;
using PlatformApi.Data;
using PlatformApi.HttpDataServices.Abstractions;
using PlatformApi.MessageBus.Abstractions;
using PlatformApi.MessageBus.Constants;
using PlatformApi.Models;
using PlatformApi.Models.Dto;
using PlatformApi.Models.MessageBus;

namespace PlatformApi.Business
{
    public class PlatformService : IPlatformService
    {
        private readonly PlatformDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _client;
        private readonly IMessageBusClient _mbClient;

        public PlatformService(PlatformDbContext context, IMapper mapper, ICommandDataClient client, IMessageBusClient mbClient)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
            _mbClient = mbClient;
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
            // try
            // {
            //     await _client.SendPlatformToCommand(result);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }

            try
            {
                var publishDto = _mapper.Map<PlatformPublishDto>(result);
                publishDto.Event = MessageBusEventConstants.PLATFORM_PUBLISHED;
                _mbClient.PublishNewPlatform(publishDto);
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
