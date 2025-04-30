namespace Frontend.Models;

public class ForumPost
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } 
    public string Content { get; set; } 
    // public User Author { get; set; }
    public User Author { get; set; } 
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public ForumCategory Category { get; set; }
    public List<ForumComment> Comments { get; set; } = new();
}