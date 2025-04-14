using Frontend.Models;

namespace Frontend.Services;

public interface IReviewService
{
    Task<Review?> GetReviewByIdAsync(string id);
    Task<List<Review>?> GetAllReviewsAsync();
    Task<bool> AddReview(Review review);
    Task<bool> UpdateReview(string id, Review review);
    Task<bool> DeleteReview(string id);
}