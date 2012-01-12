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
        private readonly IAdapter<Category, CategoryViewModel> _categoryAdapter;

        public RecipeController(IDataProvider<Recipe,string> recipeDataProvider,
                                IDataProvider<Category,string> categoryDataProvider,
                                IAdapter<Recipe,RecipeViewModel> recipeAdapter,
                                IAdapter<RecipeViewModel,Recipe> recipeViewModelAdapter,
                                IAdapter<Category,CategoryViewModel> categoryAdapter
            )
        {
            _recipeDataProvider = recipeDataProvider;
            _categoryDataProvider = categoryDataProvider;
            _recipeAdapter = recipeAdapter;
            _recipeViewModelAdapter = recipeViewModelAdapter;
            _categoryAdapter = categoryAdapter;
        }

        public ViewResult Index(string SearchText)
        {
            // Categories
            var categories = _categoryDataProvider.Get();
            var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c=>c.Description).ToArray();

            // Recipes
            var recipes = SearchText == null ? new Recipe[0] : ExecuteSearch(SearchText);
            var recipeViewModel = _recipeAdapter.Convert(recipes).ToArray();

            // Create the view model
            var model = new RecipeIndexViewModel
                            {
                                Categories = categoryViewModels,
                                SearchResults = recipeViewModel
                            };

            return View(model);
        }

        internal IEnumerable<Recipe> ExecuteSearch(string searchTerms)
        {
            var nullSafeCriteria = searchTerms.SafeToLower().SafeTrim();

            // If there was no criteria, then get all
            if (nullSafeCriteria.IsEmpty())
                return _recipeDataProvider.Get();

            // See if there were any properties specified
            var parsedSearch = nullSafeCriteria.Split('=');
            if (parsedSearch[0].SafeTrim() == "category" && parsedSearch.Length>=2)
                return _recipeDataProvider.Search(recipe => string.Equals(recipe.Category,parsedSearch[1],StringComparison.InvariantCultureIgnoreCase));
            
            // Perform the search
            return _recipeDataProvider.Search(recipe => 
                   recipe.Category.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Name.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Ingredients.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Directions.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Source.SafeToLower().SafeContains(nullSafeCriteria));
        }

    }
}
