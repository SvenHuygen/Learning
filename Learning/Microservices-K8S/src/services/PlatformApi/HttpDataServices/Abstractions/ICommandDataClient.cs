using PlatformApi.Models.Dto;

namespace PlatformApi.HttpDataServices.Abstractions
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto dto);
    }
}
