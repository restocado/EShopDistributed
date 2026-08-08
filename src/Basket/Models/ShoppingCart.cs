namespace Basket.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => 
        Items.Where(x => x.Status == ItemStatus.Available)
             .Sum(x => x.Price * x.Quantity);
}
