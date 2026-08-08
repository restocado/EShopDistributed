namespace Catalog.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/", async (IProductService service) =>
        {
            var products = await service.GetProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<ProductDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IProductService service) =>
        {
            var product = await service.GetProductAsync(id);

            if (product is null)
                return Results.NotFound();

            return Results.Ok(product);
        })
        .WithName("GetProductById")
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (ProductDto product, IProductService service) =>
        {
            var created = await service.CreateProductAsync(product);
            return Results.CreatedAtRoute("GetProductById", new { id = created.Id }, created);
        })
        .WithName("CreateProduct")
        .Produces<ProductDto>(StatusCodes.Status201Created);

        group.MapPut("/{id:int}", async (int id, ProductDto product, IProductService service) =>
        {
            var existing = await service.GetProductAsync(id);
            if (existing is null)
                return Results.NotFound();

            await service.UpdateProductAsync(id, product);
            return Results.Ok(product);
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IProductService service) =>
        {
            var existing = await service.GetProductAsync(id);
            if (existing is null)
                return Results.NotFound();

            await service.DeleteProductAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteProductById")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
