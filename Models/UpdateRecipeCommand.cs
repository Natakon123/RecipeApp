using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class UpdateRecipeCommand : EditRecipeBase
    {
        public int Id { get; set; }

        /// <summary>Photo currently stored for the recipe (display only)</summary>
        public string? CurrentPhotoPath { get; set; }

        /// <summary>Tick to delete the current photo without uploading a new one</summary>
        public bool RemovePhoto { get; set; }

        public void UpdateRecipe(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
            recipe.Method = Method ?? string.Empty;
            recipe.IsVegetarian = IsVegetarian;
            recipe.IsVegan = IsVegan;
            recipe.Servings = Servings;
        }
    }
}
