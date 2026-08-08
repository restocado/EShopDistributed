using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Basket.Services;

public class BasketService : IBasketService
{
    private readonly IDistributedCache _cache;
    private readonly IOptions<CacheSettings> _settings;

    public BasketService(IDistributedCache cache, IOptions<CacheSettings> settings)
    {
        _cache = cache;
        _settings = settings;
    }

    public async Task DeleteBasketAsync(string username)
    {
        await _cache.RemoveAsync(username);
    }

    public async Task<ShoppingCart?> GetBasketAsync(string username)
    {
        var basket = await _cache.GetStringAsync(username);
        return string.IsNullOrEmpty(basket)
            ? null
            : JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task UpdateBasketAsync(ShoppingCart basket)
    {
        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(_settings.Value.SlidingExpirationMinutes),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.Value.AbsoluteExpirationMinutes)
        };
        var json = JsonSerializer.Serialize(basket);

        await _cache.SetStringAsync(basket.UserName, json, options);
    }
}