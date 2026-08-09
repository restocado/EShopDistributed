using MassTransit;
using Shared.Messaging.Events;

namespace Basket.Messaging.Consumers;

public class ProductPriceChangedConsumer
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    private readonly IBasketService _basketService;

    public ProductPriceChangedConsumer(IBasketService basketService)
    {
        _basketService = basketService;
    }

    // Alternative implementation (commented out):
    // This version uses the event payload directly to update basket items.
    // Downside: it risks stale data if the event payload is delayed or incomplete,
    // since it bypasses the Catalog API refresh logic.
    //public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    //{
    //    var evt = context.Message;
    //
    //    var baskets = await _basketService.GetBasketsAsync();
    //
    //    foreach (var basket in baskets)
    //    {
    //        foreach (var item in basket.Items.Where(i => i.ProductId == evt.ProductId))
    //        {
    //            item.Price = evt.Price;
    //            item.ProductName = evt.Name;
    //            item.Status = ItemStatus.Available;
    //        }
    //
    //        await _basketService.UpdateBasketAsync(basket);
    //    }
    //}

    // This version ignores the payload details and simply triggers a basket refresh.
    // UpdateBasketAsync will query the Catalog API to ensure product details are always current.
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var evt = context.Message;

        var baskets = await _basketService.GetBasketsAsync();

        foreach (var basket in baskets)
        {
            // No manual item updates here — Catalog API is the source of truth.
            await _basketService.UpdateBasketAsync(basket);
        }
    }
}
