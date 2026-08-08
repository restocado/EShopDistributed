namespace Basket.ApiClients;

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
}
