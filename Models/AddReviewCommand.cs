using RecipeApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class AddReviewCommand
    {
        [Required, StringLength(50), DisplayName("Your name")]
        public string ReviewerName { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Please choose 1-5 stars")]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public Review ToReview(int recipeId) => new()
        {
            RecipeId = recipeId,
            ReviewerName = ReviewerName.Trim(),
            Rating = Rating,
            Comment = string.IsNullOrWhiteSpace(Comment) ? null : Comment.Trim(),
            CreatedAt = DateTime.UtcNow,
        };
    }
}
