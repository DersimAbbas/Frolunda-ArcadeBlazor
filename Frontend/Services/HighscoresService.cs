using Frontend.Models;
using Frontend.Services.Interfaces;
using System.Net.Http;

namespace Frontend.Services
{
    public class HighscoresService(HttpClient httpClient) : IHighscoresService
    {
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
            return await httpClient.GetFromJsonAsync<List<Highscores>>("api/highscores");
        }

        public async Task<Highscores?> GetHighscoresAsync(string highscoresId)
        {
            return await httpClient.GetFromJsonAsync<Highscores>($"api/highscores/{highscoresId}");
        }

        public async Task<Highscores?> GetHighscoresByNameAsync(string name)
        {
            return await httpClient.GetFromJsonAsync<Highscores>($"api/highscores/get-by-name/{name}");
        }

        public async Task<bool> UpdateUserHighscoreAsync(string id, Highscores highscores)
        {
            var response = await httpClient.PostAsJsonAsync($"api/highscores/{id}/update-user-score", highscores);
            return response.IsSuccessStatusCode;
        }
    }
}
