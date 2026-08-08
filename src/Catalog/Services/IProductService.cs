using Shared.Contracts;

namespace Catalog.Services;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(ProductDto product);
    Task DeleteProductAsync(int id);
    Task<ProductDto?> GetProductAsync(int id);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task UpdateProductAsync(int id, ProductDto product);
}