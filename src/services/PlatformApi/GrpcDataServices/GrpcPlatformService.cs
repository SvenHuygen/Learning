using AutoMapper;
using Grpc.Core;
using PlatformApi.Business.Abstractions;

namespace PlatformApi.GrpcDataServices
{
    public class GrpcPlatformService : GrpcPlatformApi.GrpcPlatformApiBase //Class generated with proto
    {
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcPlatformService> _logger;

        public GrpcPlatformService(IPlatformService platformService, IMapper mapper, ILogger<GrpcPlatformService> logger)
        {
            _platformService = platformService;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<GetAllPlatformsResponse> GetAllPlatforms(GetAllPlatformsRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetAllPlatforms triggered");
            var response = new GetAllPlatformsResponse();
            var platforms = await _platformService.GetAll();

            foreach (var p in platforms)
            {
                response.PlatformList.Add(_mapper.Map<GrpcPlatformReadModel>(p));
            }

            return response;
        }
    }
}