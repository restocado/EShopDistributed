namespace Basket.Endpoints;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("basket")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapGet("/{username}", async (string username, IBasketService service) =>
        {
            var basket = await service.GetBasketAsync(username);

            if (basket is null)
                return Results.NotFound();

            return Results.Ok(basket);
        })
        .WithName("GetBasket")
        .Produces<ShoppingCart>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (ShoppingCart basket, IBasketService service) =>
        {
            await service.UpdateBasketAsync(basket);
            return Results.Ok(basket);  
        })
        .WithName("UpdateBasket")
        .Produces<ShoppingCart>(StatusCodes.Status200OK);

        group.MapDelete("/{username}", async (string username, IBasketService service) =>
        {
            await service.DeleteBasketAsync(username);
            return Results.NoContent();
        })
        .WithName("DeleteBasket")
        .Produces(StatusCodes.Status200OK);

    }
}
