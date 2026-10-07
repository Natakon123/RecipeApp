using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;

namespace RecipeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RecipeService _service;

        [BindProperty(SupportsGet = true)]
        public RecipeSearchCriteria Criteria { get; set; } = new();

        public IEnumerable<RecipeSummaryViewModel> Recipes { get; private set; } = Enumerable.Empty<RecipeSummaryViewModel>();

        public IndexModel(RecipeService service)
        {
            _service = service;
        }

        public async Task OnGet()
        {
            if (Criteria.MinMinutes < 0) Criteria.MinMinutes = null;
            if (Criteria.MaxMinutes < 0) Criteria.MaxMinutes = null;
            Recipes = await _service.GetRecipes(Criteria);
        }
    }
}
