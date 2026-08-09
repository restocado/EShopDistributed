namespace Shared.Messaging.Events;

public class ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; }
    public string ImageUrl { get; init; } = default!;

    public ProductPriceChangedIntegrationEvent(
        int productId,
        string name,
        string description,
        decimal price,
        string imageUrl,
        Guid? correlationId = null,
        string? source = null
    ) : base(correlationId, source)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
    }
}
