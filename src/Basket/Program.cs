var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache(connectionName: "basket-cache");
builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddMassTransitWithAssemblies(typeof(BasketMessagingAnchor).Assembly);

builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddHttpClient<ICatalogApiClient, CatalogApiClient>(config =>
{
    config.BaseAddress = new Uri("https+http://catalog");
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapBasketEndpoints();

app.UseHttpsRedirection();

app.Run();
