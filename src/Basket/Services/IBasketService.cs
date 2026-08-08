namespace Basket.Services;

public interface IBasketService
{
    Task<ShoppingCart?> GetBasketAsync(string username);
    Task UpdateBasketAsync(ShoppingCart basket);
    Task DeleteBasketAsync(string username);
}
