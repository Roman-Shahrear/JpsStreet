namespace JpsStreet.Services.EmailApi.Messaging
{
    public interface IRabbitMQServicesConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}
