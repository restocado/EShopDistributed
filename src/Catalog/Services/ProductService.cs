namespace Catalog.Services;

public class ProductService : IProductService
{
    private readonly ProductDbContext _dbContext;

    public ProductService(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        var existing = await _dbContext.Products.FindAsync(id);
        if (existing is null)
            throw new Exception($"Product {product.Id} was not found.");

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.ImageUrl = product.ImageUrl;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product is null)
            return;

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }

}
