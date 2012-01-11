using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domus.Adapters;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Models.Recipes;
using Rolstad.Extensions;

namespace Domus.Web.UI.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IDataProvider<Recipe, string> _recipeDataProvider;
        private readonly IDataProvider<Category, string> _categoryDataProvider;
        private readonly IAdapter<Recipe, RecipeViewModel> _recipeAdapter;
        private readonly IAdapter<RecipeViewModel, Recipe> _recipeViewModelAdapter;

        public RecipeController(IDataProvider<Recipe,string> recipeDataProvider,
                                IDataProvider<Category,string> categoryDataProvider,
                                IAdapter<Recipe,RecipeViewModel> recipeAdapter,
                                IAdapter<RecipeViewModel,Recipe> recipeViewModelAdapter )
        {
            _recipeDataProvider = recipeDataProvider;
            _categoryDataProvider = categoryDataProvider;
            _recipeAdapter = recipeAdapter;
            _recipeViewModelAdapter = recipeViewModelAdapter;
        }

        public ViewResult Index(string SearchText)
        {
            var model = new RecipeIndexViewModel {Categories = new[]
                                                                   {
                                                                       new CategoryViewModel {Description = "one"}
                                                                   }};
            return View(model);
        }

        internal IEnumerable<Recipe> ExecuteSearch(string searchTerms)
        {
            var nullSafeCriteria = searchTerms.SafeToLower();

            return _recipeDataProvider.Search(recipe => 
                   recipe.Category.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Name.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Ingredients.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Directions.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Source.SafeToLower().SafeContains(nullSafeCriteria));
        }

    }
}
