using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipes
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public UpdateRecipeCommand Input { get; set; } = new();
        private readonly RecipeService _service;

        public EditModel(RecipeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var recipe = await _service.GetRecipeForUpdate(id);
            if (recipe is null)
            {
                // If id is not for a valid Recipe, generate a 404 error page
                return NotFound();
            }
            Input = recipe;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var photoError = PhotoService.Validate(Input.Photo);
            if (photoError != null) ModelState.AddModelError("Input.Photo", photoError);

            try
            {
                if (ModelState.IsValid)
                {
                    await _service.UpdateRecipe(Input);
                    return RedirectToPage("View", new { id = Input.Id });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "An error occured saving the recipe"
                    );
            }

            // Redisplay the form: the current photo path is not posted back, so reload it
            Input.CurrentPhotoPath = await _service.GetPhotoPath(Input.Id);
            return Page();
        }
    }
}
