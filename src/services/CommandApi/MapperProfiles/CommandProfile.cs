using AutoMapper;
using CommandApi.Models;
using CommandApi.Models.Dto;

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
        }
    }
}