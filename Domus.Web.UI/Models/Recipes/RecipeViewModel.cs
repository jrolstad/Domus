
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domus.Web.UI.Models.Recipes
{
    public class RecipeViewModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public virtual string RecipeId { get; set; }

        /// <summary>
        /// Name the recipe is commonly referred to as
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Number of servings this recipe produces
        /// </summary>
        public virtual string Servings { get; set; }

        /// <summary>
        /// Current rating for the recipe
        /// </summary>
        [DataType("Integer", ErrorMessage = "Rating must be a whole number")]
        public virtual int? Rating { get; set; }

        /// <summary>
        /// Category this recipe falls into
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public virtual string Category { get; set; }

        /// <summary>
        /// Ingredients used to make the food item this recipe is for
        /// </summary>
        public virtual string Ingredients { get; set; }

        /// <summary>
        /// How to make the food item
        /// </summary>
        public virtual string Directions { get; set; }

        /// <summary>
        /// Where this recipe came from
        /// </summary>
        public virtual string Source { get; set; }

        /// <summary>
        /// Image for the recipe
        /// </summary>
        public ImageViewModel Image { get; set; }

    }
}