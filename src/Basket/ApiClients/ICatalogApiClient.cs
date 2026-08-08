namespace Basket.ApiClients;

public interface ICatalogApiClient
{
    Task<ProductDto?> GetProductByIdAsync(int id);
}
