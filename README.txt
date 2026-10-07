ITMI1302 Web Application Development with C# .Net Core Framework
Laboratory #12 - EF Core + Local DB + Razor Pages (RecipeApp)

Student: Natakon Wongnikom
Student ID: 671410014

Fixed bug: _EditRecipePartial is included via <partial name="_EditRecipePartial" for="Input" />
(NOT model="Model.Input") in Create.cshtml and Edit.cshtml, so posted field names get the
"Input." prefix and correctly model-bind back into the Input property. Using model="Model.Input"
causes Name/Method/etc. to post back empty.


=== New features (per project proposal) ===
1. Recipe Search & Filter  - Home page: name/ingredient, min/max cooking time, vegetarian/vegan.
2. Rating & Reviews        - 1-5 stars + comment on the recipe page; average shown on the list.
3. Recipe Photo Upload     - Upload on Create/Edit (jpg/png/gif/webp, max 5 MB) -> wwwroot/uploads;
                             shown on the list and detail pages.
4. Portion Scaler          - Change servings on the recipe page; ingredient quantities rescale.
5. Nutrition               - Enter kcal/protein/carbs/fat per ingredient; the page shows
                             per-serving and total values.
Edit page now also edits ingredients (and fixes the missing Input.Id).

=== Database update (REQUIRED - new tables/columns) ===
In Package Manager Console:
    Add-Migration AddRecipeFeatures
    Update-Database
or CLI:
    dotnet ef migrations add AddRecipeFeatures
    dotnet ef database update
(If the Migrations folder / DB does not exist yet, run: dotnet ef database drop, then the two commands above.)
