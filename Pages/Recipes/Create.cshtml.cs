using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateRecipeCommand Input { get; set; } = new();
        private readonly RecipeService _service;

        public CreateModel(RecipeService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            Input = new CreateRecipeCommand();
        }

        public async Task<IActionResult> OnPost()
        {
            var photoError = PhotoService.Validate(Input.Photo);
            if (photoError != null) ModelState.AddModelError("Input.Photo", photoError);

            try
            {
                if (ModelState.IsValid)
                {
                    var id = await _service.CreateRecipe(Input);
                    return RedirectToPage("View", new { id = id });
                }
            }
            catch (Exception)
            {
                // Add a model-level error by using an empty string key
                ModelState.AddModelError(
                    string.Empty,
                    "An error occured saving the recipe"
                    );
            }

            //If we got to here, something went wrong
            return Page();
        }
    }
}
