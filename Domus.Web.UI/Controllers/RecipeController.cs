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
    /// <summary>
    /// Controller for recipe data management
    /// </summary>
    public class RecipeController : Controller
    {
        private readonly IDataProvider<Recipe, string> _recipeDataProvider;
        private readonly IDataProvider<Category, string> _categoryDataProvider;
        private readonly IAdapter<Recipe, RecipeViewModel> _recipeAdapter;
        private readonly IAdapter<RecipeViewModel, Recipe> _recipeViewModelAdapter;
        private readonly IAdapter<Category, CategoryViewModel> _categoryAdapter;

        /// <summary>
        /// Constructor with all dependencies
        /// </summary>
        /// <param name="recipeDataProvider">Provider for recipe data</param>
        /// <param name="categoryDataProvider">Provder for categories</param>
        /// <param name="recipeAdapter">Adapter for converting from the recipe domain model to view model</param>
        /// <param name="recipeViewModelAdapter">Adapter for converting from the recipe view model to domain model</param>
        /// <param name="categoryAdapter">Adapter for converting from the category domain model to view model</param>
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

        /// <summary>
        /// Shows the index page
        /// </summary>
        /// <param name="SearchText">Possible text to search with</param>
        /// <returns></returns>
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

        /// <summary>
        /// Shows the details (read-only) of a given recipe
        /// </summary>
        /// <param name="recipeId">Recipe to show</param>
        /// <returns></returns>
        public ViewResult Detail(string recipeId)
        {
            // Get the recipe to show
            var recipeModel = GetSelectedRecipeViewModel(recipeId);

            return View(recipeModel);

        }

        /// <summary>
        /// Shows the details of a recipe for editing
        /// </summary>
        /// <param name="recipeId">Recipe to show</param>
        /// <param name="isNew">If this is a new recipe or not</param>
        /// <returns></returns>
        public ViewResult Edit(string recipeId, bool isNew=false)
        {
            // Obtain either a new recipe or get the existing one
            var recipeModel = isNew ? this.GetNewRecipeViewModel() : GetSelectedRecipeViewModel(recipeId);

            return View(recipeModel);

        }

        /// <summary>
        /// Given a recipe, save it
        /// </summary>
        /// <param name="selectedRecipe">Recipe to save</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(SelectedRecipeViewModel selectedRecipe)
        {
            if (!ModelState.IsValid)
            {
                // Categories
                var categories = _categoryDataProvider.Get();
                var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c => c.Description).ToArray();

                selectedRecipe.Categories = categoryViewModels;

                return View("Edit", selectedRecipe);
            }

            var recipe = _recipeViewModelAdapter.Convert(selectedRecipe.Recipe);
            _recipeDataProvider.Save(recipe);

            return RedirectToAction("Detail",new{recipeId = recipe.RecipeId});
        }

        /// <summary>
        /// Creates a new recipe and forwards for user input
        /// </summary>
        /// <returns></returns>
        public RedirectToRouteResult Create()
        {
            return RedirectToAction("Edit", new { recipeId = Guid.NewGuid().ToString(), isNew = true});
        }

        /// <summary>
        /// Creates the view model for a new recipe
        /// </summary>
        /// <returns></returns>
        internal SelectedRecipeViewModel GetNewRecipeViewModel()
        {
            // Categories
            var categories = _categoryDataProvider.Get();
            var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c => c.Description).ToArray();

            // Create a new recipe
            var recipeModel = new RecipeViewModel {RecipeId = Guid.NewGuid().ToString()};

            return new SelectedRecipeViewModel
            {
                Categories = categoryViewModels,
                Recipe = recipeModel
            };
        }

        /// <summary>
        /// Creates the view model for an existing recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        internal SelectedRecipeViewModel GetSelectedRecipeViewModel(string recipeId)
        {
            // Categories
            var categories = _categoryDataProvider.Get();
            var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c=>c.Description).ToArray();

            // Obtain the given recipe
            var recipe = _recipeDataProvider.Get(recipeId);
            var recipeModel = recipe != null ?_recipeAdapter.Convert(recipe) : new RecipeViewModel {RecipeId = recipeId};

            return new SelectedRecipeViewModel
                       {
                           Categories = categoryViewModels,
                           Recipe = recipeModel
                       };
        }

        /// <summary>
        /// Performs searching for recipes given the input text
        /// </summary>
        /// <param name="searchTerms">Text to search with</param>
        /// <returns></returns>
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
