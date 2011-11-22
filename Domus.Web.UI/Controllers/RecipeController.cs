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

        public ViewResult Index(string searchTerms)
        {
            var categories = _categoryDataProvider
                .GetAll()
                .Select(c => c.Description);

            var recipes = searchTerms.IsEmpty() ?
                                new RecipeViewModel[0]
                              : this.ExecuteSearch(searchTerms)
                                    .Select(_recipeAdapter.Convert);

            var viewModel = new RecipeIndexViewModel
                                {
                                    Categories = categories,
                                    SearchText = searchTerms,
                                    SearchResults = recipes
                                };
            return View(viewModel);
        }

        public ViewResult Category(string category)
        {
            var recipes = _recipeDataProvider.Search(r => r.Category == category);
            var viewModels = _recipeAdapter.Convert(recipes);

            return View(viewModels);
        }

        public ViewResult Search(string searchTerms)
        {
            var recipes = this.ExecuteSearch(searchTerms);
            var viewModels = _recipeAdapter.Convert(recipes);

            return View(viewModels);
        }

        public ViewResult Details(string recipeId)
        {
            var recipe = _recipeDataProvider.Get(recipeId);
            var viewModel = _recipeAdapter.Convert(recipe);

            return View(viewModel);
        }

        public RedirectToRouteResult Create()
        {
            var newRecipe = new RecipeViewModel {RecipeId = Guid.NewGuid().ToString()};

            return RedirectToAction("Edit",string.Empty);
        } 

        public ViewResult Edit(string recipeId)
        {
            var recipe = _recipeDataProvider.Get(recipeId);
            var viewModel = _recipeAdapter.Convert(recipe);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save(RecipeViewModel recipeViewModel)
        {
            if(this.ModelState.IsValid)
            {
                var recipe = _recipeViewModelAdapter.Convert(recipeViewModel);
                _recipeDataProvider.Save(recipe,recipe.RecipeId);

                return RedirectToAction("Details", recipe.RecipeId);
            }

            return View("Edit", recipeViewModel);
            
        }

        public RedirectToRouteResult Delete(string recipeId)
        {
            _recipeDataProvider.Delete(recipeId);
            
            return RedirectToAction("Index");
        }

        internal IEnumerable<Recipe> ExecuteSearch(string searchTerms)
        {
            var nullSafeCriteria = searchTerms.SafeToLower();

            return _recipeDataProvider.Search(recipe => recipe.Category.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Name.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Ingredients.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Directions.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Source.SafeToLower().SafeContains(nullSafeCriteria));
        }

    }
}
