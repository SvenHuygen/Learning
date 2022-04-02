using System.Text;
using System.Text.Json;
using PlatformApi.MessageBus.Abstractions;
using PlatformApi.Models.MessageBus;
using RabbitMQ.Client;

namespace PlatformApi.MessageBus
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config, ILogger<MessageBusClient> logger)
        {
            _logger = logger;
            _config = config;

            var rabbitMQConfig = _config.GetSection("RabbitMQ").GetChildren().ToDictionary(x => x.Key, x => x.Value);

            var conFactory = new ConnectionFactory()
            {
                HostName = rabbitMQConfig.GetValueOrDefault("Host"),
                Port = int.Parse(rabbitMQConfig.GetValueOrDefault("Port"))
            };

            try
            {
                _connection = conFactory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: rabbitMQConfig.GetValueOrDefault("Exchange"), type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _logger.LogInformation("Connected to Message Bus");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishDto dto)
        {
            var message = JsonSerializer.Serialize(dto);

            if (_connection.IsOpen)
            {
                _logger.LogInformation("RabbitMQ connection is open, sending message");
                SendMessage(message);
            }
            else
            {
                _logger.LogInformation("RabbitMQ connection is closed, cannot send message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            _logger.LogInformation($"RabbitMQ has sent {message} to trigger with routingKey \"\" and basicProperties \"null\"");
        }

        public void Dispose()
        {
            _logger.LogInformation("MessageBus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            _logger.LogInformation("RabbitMQ Connection Shutdown");
        }
    }
}