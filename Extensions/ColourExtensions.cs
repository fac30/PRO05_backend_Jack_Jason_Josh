using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace J3.ColourExtensions;

public class ColourNameExtensions
{
    private readonly HttpClient _httpClient;

    public ColourNameExtensions(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetColorNameAsync(string hex)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://www.thecolorapi.com/id?hex={hex}");

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read and parse the response content
            var responseData = await response.Content.ReadAsStringAsync();
            var jsonData = JsonDocument.Parse(responseData);

            // Extract the color name from the response
            var colorName = jsonData
                .RootElement.GetProperty("name")
                .GetProperty("value")
                .GetString();

            return colorName;
        }
        catch
        {
            return null; // Return null or throw an exception based on your error handling strategy
        }
    }
}
