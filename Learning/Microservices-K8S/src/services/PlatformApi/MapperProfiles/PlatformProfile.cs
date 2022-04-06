using AutoMapper;
using PlatformApi.Models;
using PlatformApi.Models.Dto;
using PlatformApi.Models.MessageBus;

namespace PlatformApi.MapperProfiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, Platform>();

            CreateMap<PlatformCreateDto, Platform>();

            CreateMap<PlatformReadDto, PlatformPublishDto>();

            CreateMap<PlatformReadDto, GrpcPlatformReadModel>()
            .ForMember(x => x.PlatformId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Publisher, x => x.MapFrom(y => y.Publisher));
        }
    }
}
