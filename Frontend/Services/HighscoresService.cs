using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.Json;

namespace Frontend.Services;

public class HighscoresService : IHighscoresService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    private const string _highscoresKey = "highscores";
    private const string _timestampKey = "highscoresTimestamp";

    public HighscoresService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> AddNewHighscoresAsync(string name)
    {
        var response = await _httpClient.PostAsJsonAsync("api/highscores", name);

        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<Highscores>();

            var highscores = await GetAllHighscoresAsync();
            highscores.Add(created);

            var updatedJson = JsonSerializer.Serialize(highscores);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _highscoresKey, updatedJson);
        }

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteHighscoresAsync(string highscoresId)
    {
        var result = await _httpClient.DeleteAsync($"api/highscores/{highscoresId}");

        if (result.IsSuccessStatusCode)
        {
            var highscores = await GetAllHighscoresAsync();
            var toRemove = highscores.FirstOrDefault(h => h.Id == highscoresId);
            if (toRemove != null)
            {
                highscores.Remove(toRemove);

                var updatedJson = JsonSerializer.Serialize(highscores);
                await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _highscoresKey, updatedJson);
            }
        }

        return result.IsSuccessStatusCode;
    }

    public async Task<List<Highscores>?> GetAllHighscoresAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _highscoresKey);
        var timestampString = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _timestampKey);

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

        var highscores = await _httpClient.GetFromJsonAsync<List<Highscores>>("api/highscores") ?? new List<Highscores>();

        var highscoresJson = JsonSerializer.Serialize(highscores);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _highscoresKey, highscoresJson);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _timestampKey, DateTime.UtcNow.Ticks.ToString());

        return highscores;
    }

    public async Task<Highscores?> GetHighscoresAsync(string highscoresId)
    {
        return await _httpClient.GetFromJsonAsync<Highscores>($"api/highscores/{highscoresId}");
    }

    public async Task<Highscores?> GetHighscoresByNameAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<Highscores>($"api/highscores/get-by-name/{name}");
    }

    public async Task<bool> UpdateUserHighscoreAsync(string highscoresId, string userId, int score)
    {
        var dto = new
        {
            UserId = userId,
            Score = score
        };

        var response = await _httpClient.PostAsJsonAsync($"api/highscores/{highscoresId}/update-user-score", dto);

        if (response.IsSuccessStatusCode)
        {
            var highscores = await GetAllHighscoresAsync();
            var highscore = highscores.FirstOrDefault(h => h.Id == highscoresId);
            if (highscore != null)
            {
                highscore.UserScores[userId] = score;

                var updatedJson = JsonSerializer.Serialize(highscores);
                await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _highscoresKey, updatedJson);
            }
        }

        return response.IsSuccessStatusCode;
    }
}
