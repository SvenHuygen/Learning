using System.Text.Json;
using AutoMapper;
using CommandApi.Business.Abstractions;
using CommandApi.MessageBus.Constants;
using CommandApi.MessageBus.EventProcessor.Abstractions;
using CommandApi.Models.Dto;
using CommandApi.Models.MessageBus;

namespace CommandApi.MessageBus.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public void HandleEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case MessageBusEventConstants.PLATFORM_PUBLISHED:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private string DetermineEvent(string serializedEvent)
        {
            _logger.LogInformation("Determining event");

            var eventType = JsonSerializer.Deserialize<BaseEvent>(serializedEvent);

            switch (eventType.Event)
            {
                case MessageBusEventConstants.PLATFORM_PUBLISHED:
                    _logger.LogInformation("Platform published event detected.");
                    return MessageBusEventConstants.PLATFORM_PUBLISHED;
                default:
                    _logger.LogInformation("Could not determine the event type");
                    return MessageBusEventConstants.UNDETERMINED;
            }
        }

        private async void AddPlatform(string platformEventMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var platformService = scope.ServiceProvider.GetRequiredService<ICommandService>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformEventMessage);

                try
                {
                    var platformCreateDto = _mapper.Map<PlatformCreateDto>(platformPublishedDto);

                    if (await platformService.GetExternalPlatformById(platformCreateDto.ExternalId) == null)
                    {
                        await platformService.CreatePlatform(platformCreateDto);
                    }
                    else
                    {
                        _logger.LogWarning("Platform already exists.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not add platform to DB: {ex.Message}");
                }
            }
        }
    }
}