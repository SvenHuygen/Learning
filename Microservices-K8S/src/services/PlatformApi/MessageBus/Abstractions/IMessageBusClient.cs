using PlatformApi.Models.MessageBus;

namespace PlatformApi.MessageBus.Abstractions
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishDto dto);
    }
}