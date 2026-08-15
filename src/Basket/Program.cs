using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(
        serviceName: "keycloak",
        realm: "eshop",
        configureOptions: options =>
        {
            options.RequireHttpsMetadata = true;
            options.Audience = "account";

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:8080/realms/eshop"
            };
        }
    );

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();
