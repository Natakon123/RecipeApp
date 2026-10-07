using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class EditRecipeBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 24), DisplayName("Time to cook (hrs)")]
        public int TimeToCookHrs { get; set; }

        [Range(0, 59), DisplayName("Time to cook (mins)")]
        public int TimeToCookMins { get; set; }

        public string Method { get; set; } = string.Empty;

        [DisplayName("Vegetarian?")]
        public bool IsVegetarian { get; set; }

        [DisplayName("Vegan?")]
        public bool IsVegan { get; set; }

        [Range(1, 100), DisplayName("Servings")]
        public int Servings { get; set; } = 1;

        [DisplayName("Photo")]
        public IFormFile? Photo { get; set; }

        public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();
    }
}
