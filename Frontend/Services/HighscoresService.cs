using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.Json;

namespace Frontend.Services;

public class HighscoresService : IHighscoresService
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    private const string HighscoresKey = "highscores";
    private const string TimestampKey = "highscoresTimestamp";

    public HighscoresService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task<bool> AddNewHighscoresAsync(string name)
    {
        var result = await httpClient.PostAsJsonAsync("api/highscores", name);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteHighscoresAsync(string highscoresId)
    {
        var result = await httpClient.DeleteAsync($"api/highscores/{highscoresId}");
        return result.IsSuccessStatusCode;
    }

    public async Task<List<Highscores>?> GetAllHighscoresAsync()
    {
        var json = await jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", HighscoresKey);
        var timestampString = await jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", TimestampKey);

        if (!string.IsNullOrEmpty(json) && long.TryParse(timestampString, out var ticks))
        {
            var savedTime = new DateTime(ticks);
            if ((DateTime.UtcNow - savedTime).TotalHours < 24)
            {
                try
                {
                    var cachedHighscores = JsonSerializer.Deserialize<List<Highscores>>(json);
                    if (cachedHighscores != null)
                        return cachedHighscores;
                }
                catch
                {

                }
            }
        }

        var highscores = await httpClient.GetFromJsonAsync<List<Highscores>>("api/highscores") ?? new List<Highscores>();

        var highscoresJson = JsonSerializer.Serialize(highscores);
        await jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", HighscoresKey, highscoresJson);
        await jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", TimestampKey, DateTime.UtcNow.Ticks.ToString());

        return highscores;
    }

    public async Task<Highscores?> GetHighscoresAsync(string highscoresId)
    {
        return await httpClient.GetFromJsonAsync<Highscores>($"api/highscores/{highscoresId}");
    }

    public async Task<Highscores?> GetHighscoresByNameAsync(string name)
    {
        return await httpClient.GetFromJsonAsync<Highscores>($"api/highscores/get-by-name/{name}");
    }

    public async Task<bool> UpdateUserHighscoreAsync(string highscoresId, string userId, int score)
    {
        var dto = new
        {
            UserId = userId,
            Score = score
        };

        var response = await httpClient.PostAsJsonAsync($"api/highscores/{highscoresId}/update-user-score", dto);
        return response.IsSuccessStatusCode;
    }
}
