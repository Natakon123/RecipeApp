using Microsoft.EntityFrameworkCore;
using RecipeApp.Data;
using RecipeApp.Models;

namespace RecipeApp
{
    public class RecipeService
    {
        readonly AppDbContext _context;
        readonly PhotoService _photos;
        readonly ILogger _logger;

        public RecipeService(AppDbContext context, PhotoService photos, ILoggerFactory factory)
        {
            _context = context;
            _photos = photos;
            _logger = factory.CreateLogger<RecipeService>();
        }

        /// <summary>
        /// Active recipes, optionally filtered by name / ingredient, cooking time and vegetarian / vegan flags
        /// </summary>
        public async Task<List<RecipeSummaryViewModel>> GetRecipes(RecipeSearchCriteria? criteria = null)
        {
            var query = _context.Recipes.Where(r => !r.IsDeleted);

            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.Query))
                {
                    var term = criteria.Query.Trim();
                    query = query.Where(r => r.Name.Contains(term)
                                          || r.Ingredients.Any(i => i.Name.Contains(term)));
                }
                if (criteria.MinMinutes.HasValue)
                {
                    var min = TimeSpan.FromMinutes(criteria.MinMinutes.Value);
                    query = query.Where(r => r.TimeToCook >= min);
                }
                if (criteria.MaxMinutes.HasValue)
                {
                    var max = TimeSpan.FromMinutes(criteria.MaxMinutes.Value);
                    query = query.Where(r => r.TimeToCook <= max);
                }
                if (criteria.Vegetarian) query = query.Where(r => r.IsVegetarian);
                if (criteria.Vegan) query = query.Where(r => r.IsVegan);
            }

            return await query
                .OrderBy(r => r.Name)
                .Select(x => new RecipeSummaryViewModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    Time = x.TimeToCook,
                    PhotoPath = x.PhotoPath,
                    IsVegetarian = x.IsVegetarian,
                    IsVegan = x.IsVegan,
                    ReviewCount = x.Reviews.Count(),
                    AverageRating = x.Reviews.Average(rv => (double?)rv.Rating) ?? 0,
                })
                .ToListAsync();
        }

        public async Task<RecipeDetailViewModel?> GetRecipeDetail(int id)
        {
            return await _context.Recipes
                .Where(x => x.RecipeId == id)
                .Where(x => !x.IsDeleted)
                .Select(x => new RecipeDetailViewModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    Method = x.Method,
                    Time = x.TimeToCook,
                    Servings = x.Servings,
                    PhotoPath = x.PhotoPath,
                    IsVegetarian = x.IsVegetarian,
                    IsVegan = x.IsVegan,
                    Ingredients = x.Ingredients
                        .OrderBy(item => item.IngredientId)
                        .Select(item => new RecipeDetailViewModel.Item
                        {
                            Name = item.Name,
                            Quantity = item.Quantity,
                            Unit = item.Unit,
                            Calories = item.Calories,
                            Protein = item.Protein,
                            Carbs = item.Carbs,
                            Fat = item.Fat,
                        }).ToList(),
                    Reviews = x.Reviews
                        .OrderByDescending(rv => rv.CreatedAt)
                        .Select(rv => new RecipeDetailViewModel.ReviewItem
                        {
                            ReviewerName = rv.ReviewerName,
                            Rating = rv.Rating,
                            Comment = rv.Comment,
                            CreatedAt = rv.CreatedAt,
                        }).ToList(),
                })
                .SingleOrDefaultAsync();
        }

        public async Task<UpdateRecipeCommand?> GetRecipeForUpdate(int recipeId)
        {
            var recipe = await _context.Recipes
                .Include(x => x.Ingredients)
                .Where(x => x.RecipeId == recipeId)
                .Where(x => !x.IsDeleted)
                .SingleOrDefaultAsync();
            if (recipe is null) return null;

            return new UpdateRecipeCommand
            {
                Id = recipe.RecipeId,
                Name = recipe.Name,
                Method = recipe.Method,
                TimeToCookHrs = (int)recipe.TimeToCook.TotalHours,
                TimeToCookMins = recipe.TimeToCook.Minutes,
                IsVegan = recipe.IsVegan,
                IsVegetarian = recipe.IsVegetarian,
                Servings = recipe.Servings,
                CurrentPhotoPath = recipe.PhotoPath,
                Ingredients = recipe.Ingredients
                    .OrderBy(i => i.IngredientId)
                    .Select(CreateIngredientCommand.FromIngredient)
                    .ToList(),
            };
        }

        /// <summary>Path of the stored photo (used to re-show it when a form is redisplayed)</summary>
        public async Task<string?> GetPhotoPath(int recipeId)
        {
            return await _context.Recipes
                .Where(x => x.RecipeId == recipeId)
                .Select(x => x.PhotoPath)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Create a new recipe
        /// </summary>
        /// <returns>The id of the new recipe</returns>
        public async Task<int> CreateRecipe(CreateRecipeCommand cmd)
        {
            var recipe = cmd.ToRecipe();
            if (cmd.Photo is { Length: > 0 })
            {
                recipe.PhotoPath = await _photos.SaveAsync(cmd.Photo);
            }

            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        /// <summary>
        /// Updates an existing recipe (details, ingredients and photo)
        /// </summary>
        public async Task UpdateRecipe(UpdateRecipeCommand cmd)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .SingleOrDefaultAsync(r => r.RecipeId == cmd.Id);
            if (recipe == null) { throw new Exception("Unable to find the recipe"); }
            if (recipe.IsDeleted) { throw new Exception("Unable to update a deleted recipe"); }

            cmd.UpdateRecipe(recipe);

            // Replace the ingredient list with what was posted
            _context.Ingredients.RemoveRange(recipe.Ingredients);
            recipe.Ingredients = cmd.Ingredients.Select(x => x.ToIngredient()).ToList();

            // Photo: new upload replaces the old one; the checkbox just removes it
            var oldPhoto = recipe.PhotoPath;
            if (cmd.Photo is { Length: > 0 })
            {
                recipe.PhotoPath = await _photos.SaveAsync(cmd.Photo);
            }
            else if (cmd.RemovePhoto)
            {
                recipe.PhotoPath = null;
            }

            await _context.SaveChangesAsync();

            if (oldPhoto != recipe.PhotoPath) _photos.Delete(oldPhoto);
        }

        /// <summary>
        /// Marks an existing recipe as deleted (soft delete)
        /// </summary>
        public async Task DeleteRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe is null) { throw new Exception("Unable to find recipe"); }

            recipe.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        /// <summary>Adds a rating / review to an active recipe</summary>
        public async Task AddReview(int recipeId, AddReviewCommand cmd)
        {
            var exists = await _context.Recipes.AnyAsync(r => r.RecipeId == recipeId && !r.IsDeleted);
            if (!exists) { throw new Exception("Unable to find the recipe"); }

            _context.Reviews.Add(cmd.ToReview(recipeId));
            await _context.SaveChangesAsync();
        }
    }
}
