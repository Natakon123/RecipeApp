using RecipeApp.Data;

namespace RecipeApp.Models
{
    public class CreateRecipeCommand : EditRecipeBase
    {
        public Recipe ToRecipe()
        {
            return new Recipe
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method ?? string.Empty,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Servings = Servings,
                Ingredients = Ingredients.Select(x => x.ToIngredient()).ToList()
            };
        }
    }
}
