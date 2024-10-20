using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace JpsStreet.Message.RabbiMQ
{
    public class MessageRabbitMQ : IMessageRabbitMQ
    {
        private readonly string _connectionString;

        public MessageRabbitMQ(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RabbitMQConnection");
        }
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            // Create a connection to RabbitMQ
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_connectionString)
            };
            // ensure connection and dsipos channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare queue (if it doesn't exist)
                channel.QueueDeclare(queue: topic_queue_Name,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Serialize message into JSON
                var jsonMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                // Create properties if needed (e.g., for CorrelationId)
                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = Guid.NewGuid().ToString();

                // Publish message to the queue
                channel.BasicPublish(exchange: "",
                                     routingKey: topic_queue_Name,
                                     basicProperties: properties,
                                     body: body);

                Console.WriteLine($"Message sent to queue: {topic_queue_Name}");
            }
            await Task.CompletedTask;
        }
    }
}
