using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domus.Adapters;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Models;

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

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Category(string category)
        {
            var recipes = _recipeDataProvider.Search(r => r.Category == category);
            var viewModels = _recipeAdapter.Convert(recipes);

            return PartialView(viewModels);
        }

        public ViewResult Search(string searchTerms)
        {
            var recipes = _recipeDataProvider.Search(r => r.Category == searchTerms);
            var viewModels = _recipeAdapter.Convert(recipes);

            return PartialView(viewModels);
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

                RedirectToAction("Details", recipe.RecipeId);
            }
        }

        public RedirectToRouteResult Delete(string recipeId)
        {
            _recipeDataProvider.Delete(recipeId);
            
            return RedirectToAction("Index");
        }

    }
}
