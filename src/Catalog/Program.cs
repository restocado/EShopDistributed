var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");

builder.Services.AddMassTransitWithAssemblies(typeof(CatalogMessagingAnchor).Assembly);

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseMigration();
}

app.MapProductEndpoints();

app.UseHttpsRedirection();

app.Run();
