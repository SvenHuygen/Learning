using AutoMapper;
using CommandApi.Models;
using CommandApi.Models.Dto;
using CommandApi.Models.MessageBus;
using PlatformApi;

namespace CommandApi.MapperProfiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformReadDto, Platform>();

            CreateMap<Command, CommandReadDto>();
            CreateMap<Command, CommandReadDto>();

            CreateMap<CommandCreateDto, Command>();
            CreateMap<PlatformCreateDto, Platform>();

            CreateMap<PlatformPublishedDto, PlatformCreateDto>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.Id));

            CreateMap<GrpcPlatformReadModel, Platform>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.PlatformId))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Commands, x => x.Ignore());
        }
    }
}