namespace JpsStreet.Message.RabbiMQ
{
    public interface IMessageRabbitMQ
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }
}
