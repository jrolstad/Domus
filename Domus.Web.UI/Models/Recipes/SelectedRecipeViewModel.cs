using System.Collections.Generic;

namespace Domus.Web.UI.Models.Recipes
{
    public class SelectedRecipeViewModel
    {
        /// <summary>
        /// Categories to search on
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        /// <summary>
        /// Selected recipe to work on
        /// </summary>
        public RecipeViewModel Recipe { get; set; }

        /// <summary>
        /// Text to search by
        /// </summary>
        public string SearchText { get; set; } 
    }
}