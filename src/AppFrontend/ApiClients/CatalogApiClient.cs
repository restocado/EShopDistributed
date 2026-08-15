using Shared.Contracts;

namespace AppFrontend.ApiClients;

public class CatalogApiClient : ICatalogApiClient
{
    private readonly HttpClient _httpClient;

    public CatalogApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _httpClient.GetFromJsonAsync<ProductDto>($"/products/{id}");
            return product;

        }
        catch (Exception ex)
        {
            // log error
            return null;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        try
        {
            var products = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>($"/products");
            return products!;

        }
        catch (Exception ex)
        {
            // log error
            return Enumerable.Empty<ProductDto>();
        }
    }

}
