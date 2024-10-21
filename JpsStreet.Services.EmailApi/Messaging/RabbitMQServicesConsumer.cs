using JpsStreet.Services.EmailApi.Models.DTo;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JpsStreet.Services.EmailApi.Services;

namespace JpsStreet.Services.EmailApi.Messaging
{
    public class RabbitMQServicesConsumer : IRabbitMQServicesConsumer, IDisposable
    {
        private readonly string _rabbitMQConnectionString;
        private readonly string _emailCartQueue;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQServicesConsumer> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly EmailService _emailService;

        public RabbitMQServicesConsumer(IConfiguration configuration, ILogger<RabbitMQServicesConsumer> logger, EmailService emailService)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _rabbitMQConnectionString = _configuration.GetValue<string>("RabbitMQConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_rabbitMQConnectionString)
            };

            // Create the RabbitMQ connection and channel
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the queue if it doesn't already exist
            _channel.QueueDeclare(queue: _emailCartQueue,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public Task StartAsync()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // Deserialize the message into a CartDTo object
                    CartDTo objMessage = JsonConvert.DeserializeObject<CartDTo>(message);
                    // Send email and log the cart data
                    await _emailService.EmailCartAndLog(objMessage);
                    // Acknowledge that the message was processed successfully
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Log the exception using ILogger
                    _logger.LogError(ex, "Error processing RabbitMQ message.");
                }
            };

            // Start consuming messages from the queue
            _channel.BasicConsume(queue: _emailCartQueue,
                                  autoAck: false,
                                  consumer: consumer);

            _logger.LogInformation("RabbitMQ consumer started.");
            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _channel?.Close();
            _connection?.Close();
            _logger.LogInformation("RabbitMQ consumer stopped.");
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
