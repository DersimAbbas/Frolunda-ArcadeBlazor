using Frontend.Components.User.Pages;
using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface IForumService
{
    Task<List<ForumPost>?> GetAllForumPostsAsync();
    Task<ForumPost?> GetForumPostByIdAsync(string id);
    Task<bool> AddForumPostAsync(ForumPost forumPost);
    Task<bool> UpdateForumPostAsync(string id, ForumPost forumPost);
    Task<bool> DeleteForumPostAsync(string id);
    Task<bool> AddForumPostComment(string forumPostId, ForumComment forumComment);
    Task<bool> DeleteForumPostComment(string forumPostId, ForumComment forumComment);
}