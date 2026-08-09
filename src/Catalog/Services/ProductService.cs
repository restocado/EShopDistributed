using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Services;

public class ProductService : IProductService
{
    private readonly ProductDbContext _dbContext;
    private readonly IBus _bus;

    public ProductService(ProductDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var entity = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null) return null;

        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            ImageUrl = entity.ImageUrl
        };
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto product)
    {
        var entity = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl
        };

        _dbContext.Products.Add(entity);
        await _dbContext.SaveChangesAsync();

        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            ImageUrl = entity.ImageUrl
        };
    }

    public async Task UpdateProductAsync(int id, ProductDto product)
    {
        var existing = await _dbContext.Products.FindAsync(id);
        if (existing is null)
            throw new KeyNotFoundException($"Product {id} was not found.");

        bool priceChanged = existing.Price != product.Price;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.ImageUrl = product.ImageUrl;

        await _dbContext.SaveChangesAsync();

        if (priceChanged)
        {
            var evt = new ProductPriceChangedIntegrationEvent(
                productId: existing.Id,
                name: existing.Name,
                description: existing.Description,
                price: existing.Price,
                imageUrl: existing.ImageUrl,
                correlationId: Guid.NewGuid(),
                source: nameof(ProductService) 
            );
            await _bus.Publish(evt);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var entity = await _dbContext.Products.FindAsync(id);
        if (entity is null) return;

        _dbContext.Products.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
