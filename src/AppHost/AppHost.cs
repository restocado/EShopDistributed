var builder = DistributedApplication.CreateBuilder(args);

// backing services
var postgres = builder
    .AddPostgres("postgres")
    .WithContainerName("eshop-postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDatabase = postgres.AddDatabase("catalogdb");

var cache = builder
    .AddRedis("cache")
    .WithContainerName("eshop-cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// projects
var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDatabase)
    .WaitFor(catalogDatabase);

var basket = builder
    .AddProject<Projects.Basket>("basket")
    .WithReference(cache)
    .WaitFor(cache);

builder.Build().Run();
