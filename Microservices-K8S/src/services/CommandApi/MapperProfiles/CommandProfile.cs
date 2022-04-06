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
            CreateMap<CommandReadDto, Command>();

            CreateMap<CommandCreateDto, Command>();
            CreateMap<PlatformCreateDto, Platform>();

            CreateMap<PlatformCreateDto, PlatformReadDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name));

            CreateMap<PlatformPublishedDto, PlatformCreateDto>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Commands, x => x.Ignore());

            CreateMap<GrpcPlatformReadModel, Platform>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(y => Guid.Parse(y.PlatformId)))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Commands, x => x.Ignore());
        }
    }
}