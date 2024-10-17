using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public interface IDadataService
{
    Task<AddressModel> CleanAddressAsync(string address);
}

public class DadataService : IDadataService
{
    private readonly HttpClient _httpClient;

    public DadataService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<AddressModel> CleanAddressAsync(string address)
    {
        var requestBody = JsonSerializer.Serialize(new { address });
        var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync("https://dadata.ru/api/clean/address/", httpContent);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AddressModel>(jsonString);
    }
}