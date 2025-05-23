﻿using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IHighscoresService
{
    Task<List<Highscores>?> GetAllHighscoresAsync();
    Task<Highscores?> GetHighscoresAsync(string highscoreId);
    Task<bool> AddNewHighscoresAsync(string Name);
    Task<bool> UpdateUserHighscoreAsync(string highscoresId, string userId, int score);
    Task<bool> DeleteHighscoresAsync(string highscoresId);
    Task<Highscores> GetHighscoresByNameAsync(string name);
}
