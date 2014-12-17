using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using domus.data;
using domus.data.models;
using domus.mvc.Models.Categories;
using domus.mvc.Models.Recipes;

namespace domus.mvc.Controllers
{
    public class RecipeController : Controller
    {
        private readonly DomusContext _dbContext;

        public RecipeController():this(new DomusContext())
        {
            
        }
        public RecipeController(DomusContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ViewResult Index()
        {
            var viewModel = SearchRecipes(null);

            return View(viewModel);
        }

        public ActionResult Search(RecipeSearchRequest request)
        {
            var viewModel = SearchRecipes(request.SearchTerms);

            return View("Index", viewModel);
        }

        public ActionResult Detail(int recipeId)
        {
            return Content(recipeId.ToString());
        }

        private RecipeSearchResultViewModel SearchRecipes(string searchTerms)
        {
            var categories = _dbContext
                .Categories
                .ToList()
                .Select(MapCategory)
                .ToList();

            var recipes = new List<RecipeSearchViewModel>();

            if (!string.IsNullOrWhiteSpace(searchTerms))
            {
                recipes = _dbContext
                    .Recipes
                    .ToList()
                    .Select(MapRecipeSearchResult)
                    .ToList();
            }

            var viewModel = new RecipeSearchResultViewModel
            {
                Categories = categories,
                Recipes = recipes
            };
            return viewModel;
        }

        private RecipeSearchViewModel MapRecipeSearchResult(Recipe recipe)
        {
            return new RecipeSearchViewModel{Name = recipe.Name,DetailUrl = Url.Action("Detail",new{recipeId=recipe.Id})};
        }

        private CategoryViewModel MapCategory(Category category)
        {
            return new CategoryViewModel {Name = category.Name,SearchUrl = Url.Action("Search",new{searchTerms=string.Format("Category={0}",category.Name)})};
        }



      
    }
}