namespace RecipeApp.Models
{
    public class RecipeDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
        public string TimeToCook => $"{(int)Time.TotalHours}hrs {Time.Minutes}mins";
        public int Servings { get; set; } = 1;
        public string? PhotoPath { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }

        public List<Item> Ingredients { get; set; } = new();
        public List<ReviewItem> Reviews { get; set; } = new();

        public int ReviewCount => Reviews.Count;
        public double AverageRating => Reviews.Count == 0 ? 0 : Reviews.Average(r => r.Rating);

        // ---- Nutrition (sum of the ingredients that have nutrition data) ----
        public bool HasNutrition => Ingredients.Any(i => i.HasNutrition);
        public decimal TotalCalories => Ingredients.Sum(i => i.Calories ?? 0);
        public decimal TotalProtein => Ingredients.Sum(i => i.Protein ?? 0);
        public decimal TotalCarbs => Ingredients.Sum(i => i.Carbs ?? 0);
        public decimal TotalFat => Ingredients.Sum(i => i.Fat ?? 0);

        private int S => Servings < 1 ? 1 : Servings;
        public decimal CaloriesPerServing => TotalCalories / S;
        public decimal ProteinPerServing => TotalProtein / S;
        public decimal CarbsPerServing => TotalCarbs / S;
        public decimal FatPerServing => TotalFat / S;

        public class Item
        {
            public string Name { get; set; } = string.Empty;
            public decimal Quantity { get; set; }
            public string Unit { get; set; } = string.Empty;
            public decimal? Calories { get; set; }
            public decimal? Protein { get; set; }
            public decimal? Carbs { get; set; }
            public decimal? Fat { get; set; }

            public bool HasNutrition => Calories.HasValue || Protein.HasValue || Carbs.HasValue || Fat.HasValue;
        }

        public class ReviewItem
        {
            public string ReviewerName { get; set; } = string.Empty;
            public int Rating { get; set; }
            public string? Comment { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
