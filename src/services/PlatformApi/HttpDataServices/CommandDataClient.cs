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

        public CommandDataClient(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
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
                Console.WriteLine("POST OK");
            }
            else
            {
                Console.WriteLine("POST NOT SO OK :(");
            }
        }
    }
}
