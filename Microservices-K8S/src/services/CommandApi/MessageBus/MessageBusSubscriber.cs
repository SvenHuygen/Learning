using System.Text;
using CommandApi.MessageBus.EventProcessor.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandApi.MessageBus
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor, ILogger<MessageBusSubscriber> logger)
        {
            _config = config;
            _eventProcessor = eventProcessor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }

        private void InitializeRabbitMQ()
        {
            _logger.LogInformation("Initializing Message Bus");
            var rabbitMQConfig = _config.GetSection("RabbitMQ").GetChildren().ToDictionary(x => x.Key, x => x.Value);

            var conFactory = new ConnectionFactory()
            {
                HostName = rabbitMQConfig.GetValueOrDefault("Host"),
                Port = int.Parse(rabbitMQConfig.GetValueOrDefault("Port"))
            };

            _connection = conFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: rabbitMQConfig.GetValueOrDefault("Exchange"), type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                _queueName, 
                rabbitMQConfig.GetValueOrDefault("Exchange"),
                routingKey: ""
            );
            _logger.LogInformation("Listening on Message Bus");

            _connection.ConnectionShutdown += RabbitMQConnectionClosed;
        }

        private void RabbitMQConnectionClosed(object sender, ShutdownEventArgs args){
            _logger.LogInformation("Message Bus connection closed");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => 
            {
                _logger.LogInformation("Event Received.");

                var body = ea.Body;
                var eventMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.HandleEvent(eventMessage);
            };

            _channel.BasicConsume(_queueName, true, consumer);

            return Task.CompletedTask;
        }
    }
}