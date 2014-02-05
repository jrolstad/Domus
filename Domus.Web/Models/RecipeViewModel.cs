namespace Domus.Web.Models
{
    public class RecipeViewModel
    {
        public string RecipeId { get; set; }

        public string Name { get; set; }

        public int Servings { get; set; }

        public decimal? Rating { get; set; }

        public string Category { get; set; }

        public string Ingredients { get; set; }

        public string Directions { get; set; }

        public string Source { get; set; }

        public string ImageUrl { get; set; }

        public string RecipeTitle { get; set; }
    }
}