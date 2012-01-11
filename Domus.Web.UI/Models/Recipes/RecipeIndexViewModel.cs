using System.Collections.Generic;

namespace Domus.Web.UI.Models.Recipes
{
    public class RecipeIndexViewModel
    {
        /// <summary>
        /// Categories to pick from
        /// </summary>
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        /// <summary>
        /// Results of the search
        /// </summary>
        public IEnumerable<RecipeViewModel> SearchResults { get; set; }

        /// <summary>
        /// Text to search by
        /// </summary>
        public string SearchText { get; set; }
    }
}