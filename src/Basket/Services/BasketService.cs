using Basket.ApiClients;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Basket.Services;

public class BasketService : IBasketService
{
    private readonly IDistributedCache _cache;
    private readonly ICatalogApiClient _catalogApiClient;
    private readonly IOptions<CacheSettings> _settings;

    public BasketService(
        IDistributedCache cache,
        ICatalogApiClient catalogApiClient,
        IOptions<CacheSettings> settings)
    {
        _cache = cache;
        _catalogApiClient = catalogApiClient;
        _settings = settings;
    }

    public async Task DeleteBasketAsync(string username)
    {
        await _cache.RemoveAsync(username);
    }

    public async Task<IEnumerable<ShoppingCart>> GetBasketsAsync()
    {
        // For demo purposes: just return one basket
        // test username
        string username = "john_doe";
        var basket = await GetBasketAsync(username);

        if (basket is null)
            return Enumerable.Empty<ShoppingCart>();

        return new List<ShoppingCart> { basket };
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
        var productTasks = basket.Items.Select(async item =>
        {
            var product = await _catalogApiClient.GetProductByIdAsync(item.ProductId);

            if (product is null)
            {
                item.Status = ItemStatus.NotAvailable;
            }
            else
            {
                item.Price = product.Price;
                item.ProductName = product.Name;
                item.Status = ItemStatus.Available;
            }

            return item;
        });

        var updatedItems = await Task.WhenAll(productTasks);
        basket.Items = updatedItems.ToList();

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(_settings.Value.SlidingExpirationMinutes),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.Value.AbsoluteExpirationMinutes)
        };

        var json = JsonSerializer.Serialize(basket);
        await _cache.SetStringAsync(basket.UserName, json, options);
    }

}