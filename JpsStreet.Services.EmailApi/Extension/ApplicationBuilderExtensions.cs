using JpsStreet.Services.EmailApi.Messaging;

namespace JpsStreet.Services.EmailApi.Extension
{
    public static class ApplicationBuilderExtensions
    {
        private static IRabbitMQServicesConsumer ServiceConsumer { get; set; }

        public static IApplicationBuilder UseRabbitMQServiceBusConsumer(this IApplicationBuilder app)
        {
            // Get the RabbitMQ service consumer from the DI container
            ServiceConsumer = app.ApplicationServices.GetService<IRabbitMQServicesConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            // Register the start and stop event handlers
            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            // Start the RabbitMQ consumer
            ServiceConsumer.StartAsync().Wait();
        }

        private static void OnStop()
        {
            // Stop the RabbitMQ consumer
            ServiceConsumer.StopAsync().Wait();
        }
    }
}
