using AutoMapper;
using CommandApi.GrpcDataServices.Abstractions;
using CommandApi.Models;
using Grpc.Net.Client;
using PlatformApi;

namespace CommandApi.GrpcDataServices
{
    public class PlatformGrpcClient : IPlatformGrpcClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformGrpcClient> _logger;

        public PlatformGrpcClient(IConfiguration config, IMapper mapper, ILogger<PlatformGrpcClient> logger)
        {
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
        {
            var GrpcConfig = _config.GetSection("Grpc").GetChildren().ToDictionary(x => x.Key, x => x.Value);
            _logger.LogInformation($"Calling gRPC service for Platforms");
            var channel = GrpcChannel.ForAddress(GrpcConfig.GetValueOrDefault("GrpcPlatform"));
            var client = new GrpcPlatformApi.GrpcPlatformApiClient(channel);
            var request = new GetAllPlatformsRequest();

            try
            {
                var response = await client.GetAllPlatformsAsync(request);
                _logger.LogInformation($"gRPC Raw Response: {response}");
                var mapped = _mapper.Map<List<Platform>>(response.PlatformList);

                return _mapper.Map<IEnumerable<Platform>>(response.PlatformList);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not call gRPC Server {ex.Message}");
                return null;
            }
        }
    }
}