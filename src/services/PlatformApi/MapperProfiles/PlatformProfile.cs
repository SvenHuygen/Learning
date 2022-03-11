using AutoMapper;
using PlatformApi.Models;
using PlatformApi.Models.Dto;

namespace PlatformApi.MapperProfiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, Platform>();

            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}
