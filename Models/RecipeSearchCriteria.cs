using System.ComponentModel;

namespace RecipeApp.Models
{
    public class RecipeSearchCriteria
    {
        [DisplayName("Name or ingredient")]
        public string? Query { get; set; }

        [DisplayName("Min time (mins)")]
        public int? MinMinutes { get; set; }

        [DisplayName("Max time (mins)")]
        public int? MaxMinutes { get; set; }

        public bool Vegetarian { get; set; }
        public bool Vegan { get; set; }

        public bool HasFilters =>
            !string.IsNullOrWhiteSpace(Query) || MinMinutes.HasValue || MaxMinutes.HasValue || Vegetarian || Vegan;
    }
}
