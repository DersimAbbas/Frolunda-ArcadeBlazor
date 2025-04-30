namespace Frontend.Models;

public class ForumComment
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Comment { get; set; }
    public User User { get; set; }
    public DateTime Created { get; set; }
}