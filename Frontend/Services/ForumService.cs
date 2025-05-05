using System.Text;
using System.Text.Json;
using Frontend.Components.User.Pages;
using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Services;

public class ForumService(HttpClient httpClient) : IForumService
{
    private IForumService _forumServiceImplementation;

    public async Task<List<ForumPost>?> GetAllForumPostsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ForumPost>>("api/forumposts");
    }

    public async Task<ForumPost?> GetForumPostByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<ForumPost>($"api/forumposts/{id}");
    }

    public async Task<bool> AddForumPostAsync(ForumPost forumPost)
    {
        var response = await httpClient.PostAsJsonAsync("api/forumposts", forumPost);
        Console.WriteLine(JsonSerializer.Serialize(forumPost));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateForumPostAsync(string id, ForumPost forumPost)
    {
        var response = await httpClient.PutAsJsonAsync($"api/forumposts/{id}", forumPost);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteForumPostAsync(string id)
    {
        var response = await httpClient.DeleteAsync($"api/forumposts/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddForumPostComment(string forumPostId, ForumComment forumComment)
    {
        var response = await httpClient.PostAsJsonAsync($"api/forumposts/{forumPostId}/comments", forumComment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteForumPostComment(string forumPostId, [FromBody] ForumComment forumComment)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/forumposts/{forumPostId}/comment/{forumComment.Id}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(forumComment),
                Encoding.UTF8,
                "application/json"
            )
        };

        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}