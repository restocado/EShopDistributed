namespace Catalog.Services;

public interface IProductService
{
    Task CreateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<Product?> GetProductAsync(int id);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task UpdateProductAsync(int id, Product product);
}