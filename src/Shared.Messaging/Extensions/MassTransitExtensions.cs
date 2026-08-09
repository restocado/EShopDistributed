using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions;

public static class MassTransitExtensions
{
    // Extension method for IServiceCollection to register MassTransit with assemblies.
    // Example usage: services.AddMassTransitWithAssemblies(typeof(SomeConsumer).Assembly);
    public static IServiceCollection AddMassTransitWithAssemblies(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            // Endpoint names are formatted in kebab-case (e.g., "product-price-changed-event").
            // This ensures consistent queue naming conventions.
            config.SetKebabCaseEndpointNameFormatter();

            // Saga state persistence is configured to use an in-memory repository.
            // Suitable for development and testing; a durable store should be used in production.
            config.SetInMemorySagaRepositoryProvider();

            /*
            // Example: EF Core saga repository with PostgreSQL for production use.
            // Requires a DbContext implementing SagaDbContext with saga mappings.
            config.AddSagaStateMachine<OrderStateMachine, OrderState>()
                  .EntityFrameworkRepository(r =>
                  {
                      r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                      r.AddDbContext<SagaDbContext, SagaDbContext>();
                  });
            */

            // Consumers, saga state machines, sagas, and activities are automatically registered
            // from the provided assemblies.
            config.AddConsumers(assemblies);
            config.AddSagaStateMachines(assemblies);
            config.AddSagas(assemblies);
            config.AddActivities(assemblies);

            // RabbitMQ is configured as the transport.
            config.UsingRabbitMq((context, configurator) =>
            {
                // RabbitMQ connection string is retrieved from application configuration.
                var configuration = context.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("rabbitmq");

                // RabbitMQ host is configured using the connection string
                // (e.g., amqp://guest:guest@localhost:5672).
                configurator.Host(connectionString);

                // Endpoints are automatically configured for all registered consumers and sagas.
                // MassTransit creates queues and exchanges and binds them to the appropriate consumers.
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
