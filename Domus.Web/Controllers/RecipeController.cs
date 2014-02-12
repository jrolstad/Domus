using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domus.Web.Models;
using Domus.Web.Models.Api;

namespace Domus.Web.Controllers
{
    public class RecipeController : Controller
    {
        private CategoryApiController _categoryApiController = new CategoryApiController();
        private RecipeApiController _recipeApiController = new RecipeApiController();
        private RecipeSearchApiController _recipeSearchApiController = new RecipeSearchApiController();

        public ViewResult Index(string category, string searchTerms)
        {
            var viewModel = BuildIndexPage();

            var searchRequest = new RecipeSearchRequest {Category = category, SearchTerms = searchTerms};
            var searchResults = _recipeSearchApiController.Get(searchRequest);
            var searchResultModels = searchResults.Select(MapSearchResult).ToList();
            viewModel.SearchResults = searchResultModels;

            return View(viewModel);
        }

        private RecipeIndexViewModel BuildIndexPage()
        {
            var categories = _categoryApiController
                .Get()
                .ToList();

            var viewModel = new RecipeIndexViewModel { Categories = categories };
            return viewModel;
        }

        public ViewResult CreateNewRecipe()
        {
            var viewModel = new RecipeViewModel
            {
                RecipeId = Guid.NewGuid().ToString(),
                RecipeTitle = "New Recipe"
            };

            return View("Edit", viewModel);
        }

        public ActionResult SaveRecipe(RecipeViewModel recipe)
        {
            var apiModel = Map(recipe);
            _recipeApiController.Post(apiModel);

           return RedirectToAction("Index");
        }

        private static RecipeApiModel Map(RecipeViewModel toMap)
        {
            return new RecipeApiModel
            {
                Category = toMap.Category,
                Directions = toMap.Directions,
                ImageUrl = toMap.ImageUrl,
                Ingredients = toMap.Ingredients,
                Name = toMap.Name,
                Rating = toMap.Rating,
                RecipeId = toMap.RecipeId,
                Servings = toMap.Servings,
                Source = toMap.Source
            };
        }

        private static RecipeViewModel Map(RecipeApiModel toMap)
        {
            return new RecipeViewModel
            {
                Category = toMap.Category,
                Directions = toMap.Directions,
                ImageUrl = toMap.ImageUrl,
                Ingredients = toMap.Ingredients,
                Name = toMap.Name,
                Rating = toMap.Rating,
                RecipeId = toMap.RecipeId,
                Servings = toMap.Servings,
                Source = toMap.Source,
                RecipeTitle = toMap.Name
            };
        }

        private static RecipeSearchResult MapSearchResult(RecipeApiModel toMap)
        {
            return new RecipeSearchResult
            {
                Name = toMap.Name,
           
                RecipeId = toMap.RecipeId,
                Rating = toMap.Rating
            };
        }

        public ViewResult RecipeDetail(string recipeid)
        {
            var recipe = _recipeApiController.Get(recipeid);
            var recipeViewModel = Map(recipe);

            return View("Detail", recipeViewModel);
        }

        public ViewResult EditRecipe(string recipeid)
        {
            var recipe = _recipeApiController.Get(recipeid);
            var categories = _categoryApiController.Get();

            var recipeViewModel = Map(recipe);
            recipeViewModel.AvailableCategories = categories.Select(c => c.Decription).ToList();

            return View("Edit", recipeViewModel);
        }
    }
}
