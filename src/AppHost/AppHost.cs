var builder = DistributedApplication.CreateBuilder(args);

// backing services | infrastructure
var postgres = builder
    .AddPostgres("postgres")
    .WithContainerName("eshop-postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDatabase = postgres.AddDatabase("catalogdb");

var basketDatabase = builder
    .AddRedis("basket-cache")
    .WithContainerName("eshop-cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithContainerName("eshop-rabbitmq")
    .WithManagementPlugin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// projects | services
var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDatabase).WaitFor(catalogDatabase)
    .WithReference(rabbitmq).WaitFor(rabbitmq);

var basket = builder
    .AddProject<Projects.Basket>("basket")
    .WithReference(catalog)
    .WithReference(basketDatabase).WaitFor(basketDatabase)
    .WithReference(rabbitmq).WaitFor(rabbitmq);

// frontend

builder.Build().Run();
