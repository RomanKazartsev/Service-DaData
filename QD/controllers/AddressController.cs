using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AddressController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("standardize")]
        public async Task<IActionResult> StandardizeAddress([FromHeader] string apiKey, [FromBody] AddressRequest request)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("API key is required.");
            }

            var standardizedAddress = await CleanAddressAsync(apiKey, request.Address);
            return Ok(standardizedAddress);
        }

        private async Task<string> CleanAddressAsync(string apiKey, string address)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://clean.dadata.ru/api/v2/clean/address");
            request.Headers.Add("Authorization", $"Token {apiKey}");

            var jsonContent = JsonSerializer.Serialize(new { source = address });
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody; // Или десериализация в модель
        }
    }

    public class AddressRequest
    {
        public string Address { get; set; }
    }
}
