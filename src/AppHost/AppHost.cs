var builder = DistributedApplication.CreateBuilder(args);

// add projects and cloud-native backing services

builder.Build().Run();
