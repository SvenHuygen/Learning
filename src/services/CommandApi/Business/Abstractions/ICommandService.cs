using CommandApi.Models.Dto;

namespace CommandApi.Business.Abstractions
{
    public interface ICommandService
    {

        Task<IEnumerable<PlatformReadDto>> GetAllPlatforms();

        Task<PlatformReadDto> GetPlatformById(Guid platformId);

        Task<PlatformReadDto> CreatePlatform(PlatformCreateDto dto);

        Task<IEnumerable<CommandReadDto>> GetCommandsForPlatform(Guid platformId);

        Task<CommandReadDto> GetCommand(Guid platformId, Guid commandId);

        Task<CommandReadDto> CreateCommand(Guid platformId, CommandCreateDto dto);
    }
}