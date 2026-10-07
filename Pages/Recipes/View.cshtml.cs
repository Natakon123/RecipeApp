using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        public RecipeDetailViewModel Recipe { get; set; } = new();

        [BindProperty]
        public AddReviewCommand Review { get; set; } = new();

        private readonly RecipeService _service;

        public ViewModel(RecipeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var recipe = await _service.GetRecipeDetail(id);
            if (recipe is null)
            {
                // If id is not for a valid Recipe, generate a 404 error page
                return NotFound();
            }
            Recipe = recipe;
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _service.DeleteRecipe(id);

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostReviewAsync(int id)
        {
            var recipe = await _service.GetRecipeDetail(id);
            if (recipe is null) return NotFound();
            Recipe = recipe;

            if (!ModelState.IsValid) return Page();

            await _service.AddReview(id, Review);
            return RedirectToPage("View", new { id });
        }
    }
}
