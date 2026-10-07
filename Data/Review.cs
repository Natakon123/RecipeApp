namespace RecipeApp.Data
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int RecipeId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }          // 1-5 stars
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
