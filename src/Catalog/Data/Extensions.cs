namespace Catalog.Data;

public static class Extensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        
        // apply migrations
        context.Database.Migrate();

        // seed products table
        DataSeeder.Seed(context);
    }
}

public class DataSeeder
{
    public static void Seed(ProductDbContext dbContext)
    {
        if (dbContext.Products.Any()) return;

        dbContext.Products.AddRange(SampleProducts);

        dbContext.SaveChanges();
    }

    public static IEnumerable<Product> SampleProducts =
    [
        new Product
        {
            Name = "Logitech G305 Lightspeed Wireless Gaming Mouse",
            Description = "Lightweight wireless gaming mouse with HERO sensor and 12,000 DPI.",
            Price = 2999.00m,
            ImageUrl = "https://www.amazon.com/dp/B07FNJB8TT" // Wireless Mouse
        },
        new Product
        {
            Name = "Logitech MX Mechanical Wireless Illuminated Keyboard",
            Description = "Backlit mechanical keyboard with tactile switches, Bluetooth and USB-C.",
            Price = 8999.00m,
            ImageUrl = "https://www.amazon.com/dp/B09VYJ7J6W" // Mechanical Keyboard
        },
        new Product
        {
            Name = "Sony WH-CH720N Wireless Noise Cancelling Headphones",
            Description = "Comfortable over-ear headphones with dual noise sensor technology.",
            Price = 5999.00m,
            ImageUrl = "https://www.amazon.com/dp/B0BXMZ9Y8V" // Noise Cancelling Headphones
        },
        new Product
        {
            Name = "Dell 27 Monitor S2725QS 4K UHD IPS",
            Description = "27-inch 4K UHD monitor with 120Hz refresh rate and FreeSync Premium.",
            Price = 15999.00m,
            ImageUrl = "https://www.amazon.com/dp/B0D2Z9K7V7" // 4K Monitor
        },
        new Product
        {
            Name = "Samsung T7 Portable SSD 1TB",
            Description = "USB 3.2 Gen 2 external SSD with up to 1,050MB/s transfer speeds.",
            Price = 4999.00m,
            ImageUrl = "https://www.amazon.com/dp/B0874Y7M3V" // Portable SSD
        }
    ];
}