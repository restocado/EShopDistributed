namespace Catalog.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions options) : base(options)
    {
    }

    protected ProductDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configs all at once:
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products => Set<Product>();
}
