using PlatformApi.Models.Dto;

namespace PlatformApi.Business.Abstractions
{
    public interface IPlatformService
    {
        Task<IEnumerable<PlatformReadDto>> GetAll();

        Task<PlatformReadDto> GetById(Guid id);

        Task<PlatformReadDto> Create(PlatformCreateDto dto);

        Task<bool> DeleteAll();
    }
}
