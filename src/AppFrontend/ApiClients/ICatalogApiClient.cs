using Shared.Contracts;

namespace AppFrontend.ApiClients;

public interface ICatalogApiClient
{
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}
