using RecipeApp.Data;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class CreateIngredientCommand
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000)]
        public decimal Quantity { get; set; }

        [StringLength(20)]
        public string? Unit { get; set; }

        // Optional nutrition for the quantity above (whole recipe, not per serving)
        [Range(0, 1000000), Display(Name = "kcal")]
        public decimal? Calories { get; set; }

        [Range(0, 1000000), Display(Name = "Protein (g)")]
        public decimal? Protein { get; set; }

        [Range(0, 1000000), Display(Name = "Carbs (g)")]
        public decimal? Carbs { get; set; }

        [Range(0, 1000000), Display(Name = "Fat (g)")]
        public decimal? Fat { get; set; }

        public Ingredient ToIngredient()
        {
            return new Ingredient
            {
                Name = Name,
                Quantity = Quantity,
                Unit = Unit ?? string.Empty,
                Calories = Calories,
                Protein = Protein,
                Carbs = Carbs,
                Fat = Fat,
            };
        }

        public static CreateIngredientCommand FromIngredient(Ingredient i) => new()
        {
            Name = i.Name,
            Quantity = i.Quantity,
            Unit = i.Unit,
            Calories = i.Calories,
            Protein = i.Protein,
            Carbs = i.Carbs,
            Fat = i.Fat,
        };
    }
}
