namespace RecipeApp.Models
{
    public class RecipeSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
        public string TimeToCook => $"{(int)Time.TotalHours}hrs {Time.Minutes}mins";
        public string? PhotoPath { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
