using PlatformApi.HttpDataServices.Abstractions;
using PlatformApi.Models.Dto;
using System.Text;
using System.Text.Json;

namespace PlatformApi.HttpDataServices
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly ILogger<CommandDataClient> _logger;

        public CommandDataClient(HttpClient client, IConfiguration config, ILogger<CommandDataClient> logger)
        {
            _client = client;
            _config = config;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformReadDto dto)
        {
            var content = new StringContent(
                        JsonSerializer.Serialize(dto),
                        Encoding.UTF8,
                        "application/json"
                    );

            var httpServiceUrls = _config.GetSection("HttpServiceUrls").GetChildren().ToDictionary(x => x.Key, x => x.Value);
            
            var response = await _client.PostAsync(httpServiceUrls.GetValueOrDefault("CommandApi"), content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("POST OK");
            }
            else
            {
                _logger.LogInformation("POST NOT OK");
            }
        }
    }
}
