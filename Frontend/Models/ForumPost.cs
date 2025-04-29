namespace Frontend.Models;

public class ForumPost
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    // public User Author { get; set; }
    public User Author { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public ForumCategory Category { get; set; }
    List<ForumComment> Comments { get; set; } = new();
}