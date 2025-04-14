using Frontend.Models;

namespace Frontend.Services;

public class ReviewService(HttpClient httpClient) : IReviewService
{
    public async Task<Review?> GetReviewByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<Review>($"api/reviews/{id}");
    }

    public async Task<List<Review>?> GetAllReviewsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Review>>("api/reviews");
    }

    public async Task<bool> AddReview(Review review)
    {
        var response = await httpClient.PostAsJsonAsync("api/reviews", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReview(string id, Review review)
    {
        var response = await httpClient.PutAsJsonAsync($"api/reviews/{id}", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReview(string id)
    {
        var response = await httpClient.DeleteAsync($"api/reviews/{id}");
        return response.IsSuccessStatusCode;
    }
}