using Google.Cloud.Firestore;

namespace Frontend.Models;

public class Highscores
{
    public string Id { get; set; }
    public string Name { get; set; }
    //Change from user id to username when implemented
    public Dictionary<string, int> UserScores { get; set; } = new();
}
