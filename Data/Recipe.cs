namespace RecipeApp.Data
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan TimeToCook { get; set; }
        public bool IsDeleted { get; set; }
        public string Method { get; set; } = string.Empty;
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }

        // --- New features ---
        /// <summary>Number of servings the ingredient quantities are written for (Portion Scaler base)</summary>
        public int Servings { get; set; } = 1;

        /// <summary>Relative path of the uploaded photo, e.g. /uploads/abc.jpg (null = no photo)</summary>
        public string? PhotoPath { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
