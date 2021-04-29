using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shidozufu.EventBus
{
    public static class Extensions
    {
        public static IServiceCollection UsePlainRabbitMq(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            services.Configure<RabbitMqOptions>(
                configuration.GetSection(nameof(RabbitMqOptions)),
                opt => configuration.Bind(opt));

            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<IPublisher, Publisher>();
            services.AddSingleton<ISubscriber, Subscriber>();

            return services;
        }
    }
}
