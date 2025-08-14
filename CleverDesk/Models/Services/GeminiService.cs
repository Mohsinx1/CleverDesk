using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class GeminiService
{
    private readonly HttpClient _httpClient;
   private const string apiKey = "YOUR GEMINI API KEY HERE";

    public GeminiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetGeminiResponseAsync(string prompt)
    {
        var request = new
        {
            contents = new[]
            {
                new {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}",
            content);

        if (!response.IsSuccessStatusCode)
        {
            return "Error contacting Gemini API.";
        }

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var reply = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return reply ?? "No response.";
    }
}
