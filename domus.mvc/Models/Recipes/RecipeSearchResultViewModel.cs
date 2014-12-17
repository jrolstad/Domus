using System.Collections.Generic;
using domus.mvc.Models.Categories;

namespace domus.mvc.Models.Recipes
{
    public class RecipeSearchResultViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }

        public List<RecipeSearchViewModel> Recipes { get; set; } 
    }
}