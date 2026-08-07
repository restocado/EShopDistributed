var builder = DistributedApplication.CreateBuilder(args);

// backing services
var postgres = builder
    .AddPostgres("postgres")
    .WithContainerName("eshop-postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDatabase = postgres.AddDatabase("catalogdb");

// projects
builder.AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDatabase)
    .WaitFor(catalogDatabase);

builder.Build().Run();
