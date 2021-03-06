﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Domus.Entities;
using Domus.Mappers;
using Domus.Providers;
using Domus.Web.UI.Models.Recipes;
using Rolstad.Extensions;
using Rolstad.MVC.Errors;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for recipe data management
    /// </summary>
    [HandleErrorAndLog]
    public class RecipeController : Controller
    {
        private readonly IRepository<Recipe, string> _recipeRepository;
        private readonly IRepository<Category, string> _categoryRepository;
        private readonly IMapper<Recipe, RecipeViewModel> _recipeMapper;
        private readonly IMapper<RecipeViewModel, Recipe> _recipeViewModelMapper;
        private readonly IMapper<Category, CategoryViewModel> _categoryMapper;
        private readonly IImageProvider _tempImageProvider;
        private readonly IFileProvider _amazonS3FileProvider;
        private readonly IFeatureUsageNotifier _featureUsageNotifier;

        public RecipeController(IRepository<Recipe,string> recipeRepository,
                                IRepository<Category,string> categoryRepository,
                                IMapper<Recipe,RecipeViewModel> recipeMapper,
                                IMapper<RecipeViewModel,Recipe> recipeViewModelMapper,
                                IMapper<Category,CategoryViewModel> categoryMapper,
                                IImageProvider tempImageProvider,
                                IFileProvider amazonS3FileProvider,
                                IFeatureUsageNotifier featureUsageNotifier
            )
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _recipeMapper = recipeMapper;
            _recipeViewModelMapper = recipeViewModelMapper;
            _categoryMapper = categoryMapper;
            _tempImageProvider = tempImageProvider;
            _amazonS3FileProvider = amazonS3FileProvider;
            _featureUsageNotifier = featureUsageNotifier;
        }

        public ViewResult Index(string SearchText)
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                _featureUsageNotifier.Notify(Feature.RecipeIndex, notes: string.Format("SearchText|{0}", SearchText));
            }

            var model = SearchRecipes(SearchText);

            return View(model);
        }

        public ViewResult Detail(string recipeId)
        {
            // Get the recipe to show
            var recipeModel = GetSelectedRecipeViewModel(recipeId);

            _featureUsageNotifier.Notify(Feature.RecipeDetail, notes: string.Format("{0}|{1}", recipeModel.Recipe.Name, recipeId));

            return View(recipeModel);

        }

        [Authorize]
        public ViewResult Edit(string recipeId, bool isNew = false)
        {
            // Obtain either a new recipe or get the existing one
            var recipeModel = EditRecipe(recipeId, isNew);

            _featureUsageNotifier.Notify(Feature.RecipeEdit, notes: string.Format("{0}|{1}", recipeModel.Recipe.Name, recipeId));

            return View(recipeModel);

        }

        [HttpPost]
        public ActionResult Save(SelectedRecipeViewModel selectedRecipe)
        {
            if (!ModelState.IsValid)
            {
                AddCategoriesToSelectedRecipeViewModel(selectedRecipe);
                return View("Edit", selectedRecipe);
            }

            _featureUsageNotifier.Notify(Feature.RecipeSave, notes: string.Format("{0}|{1}", selectedRecipe.Recipe.Name, selectedRecipe.Recipe.RecipeId));

            var recipeToSave = SaveRecipe(selectedRecipe);

            return RedirectToAction("Detail", new { recipeId = recipeToSave.RecipeId });
        }

        [Authorize]
        public RedirectToRouteResult Create()
        {
            _featureUsageNotifier.Notify(Feature.RecipeCreate);

            return RedirectToAction("Edit", new { recipeId = Guid.NewGuid().ToString(), isNew = true });
        }

        public RedirectToRouteResult Refresh()
        {
            _featureUsageNotifier.Notify(Feature.RecipeRefresh);

            RefreshRecipeData();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddImage(RecipeImageViewModel recipeViewModel)
        {
            _featureUsageNotifier.Notify(Feature.RecipeAddImage, notes: string.Format("{0}|{1}", recipeViewModel.Name, recipeViewModel.RecipeId));

            // Get the image
            var image = WebImage.GetImageFromRequest();

            // If there isn't one, re-show the details
            if (image == null)
                return RedirectToAction("Detail", new { recipeId = recipeViewModel.RecipeId });

            SaveTemporaryImage(recipeViewModel, image);

            // Once saved, send so we can crop the image
            return View("ImageCrop", recipeViewModel);
        }

        [Authorize]
        public ActionResult ImageEdit(string recipeId, string name)
        {
            var recipeImageViewModel = new RecipeImageViewModel
            {
                RecipeId = recipeId,
                Name = name

            };
            return View(recipeImageViewModel);
        }

        [Authorize]
        public ActionResult SaveCrop(RecipeImageViewModel viewModel)
        {
            SaveCroppedImageToRecipe(viewModel);

            return RedirectToAction("Detail", new { recipeId = viewModel.RecipeId });
        }

        private void SaveCroppedImageToRecipe(RecipeImageViewModel viewModel)
        {
            // Get the temp image
            var tempFileName = Path.GetFileName(viewModel.ImageUrl);
            var tempFilePath = _tempImageProvider.GetFilePath(tempFileName);
            var image = new WebImage(tempFilePath);

            // Crop the image with the specified dimensions
            CropImage(viewModel, image, 300);

            // Save the image
            var recipeImageFileName = "{0}{1}".StringFormat(viewModel.RecipeId, Path.GetExtension(tempFilePath));
            var recipeImageFilePath = _tempImageProvider.Save(image, recipeImageFileName);
            var recipeImageUrl = _amazonS3FileProvider.Save(recipeImageFilePath, "DomusRecipeImages");

            // Place the image onto the recipe
            var recipe = _recipeRepository.Get(viewModel.RecipeId);
            recipe.ImageUrl = recipeImageUrl;
            _recipeRepository.Save(recipe);

            // Delete temporary files
            _tempImageProvider.Delete(recipeImageFileName);
            _tempImageProvider.Delete(tempFileName);
        }

        private Recipe SaveRecipe(SelectedRecipeViewModel selectedRecipe)
        {
            var recipeToSave = _recipeViewModelMapper.Map(selectedRecipe.Recipe);

            var existingRecipe = _recipeRepository.Get(selectedRecipe.Recipe.RecipeId);
            if (existingRecipe != null && existingRecipe.Category != recipeToSave.Category)
            {
                recipeToSave.PreviousCategory = existingRecipe.Category;
            }

            _recipeRepository.Save(recipeToSave);
            return recipeToSave;
        }

        private void AddCategoriesToSelectedRecipeViewModel(SelectedRecipeViewModel selectedRecipe)
        {
            var categories = _categoryRepository.Get();
            var categoryViewModels = _categoryMapper.Map(categories).OrderBy(c => c.Description).ToArray();

            selectedRecipe.Categories = categoryViewModels;
        }

        private RecipeIndexViewModel SearchRecipes(string SearchText)
        {
            // Categories
            var categories = _categoryRepository.Get();
            var categoryViewModels = _categoryMapper.Map(categories).OrderBy(c => c.Description).ToArray();

            // Recipes
            var recipes = SearchText == null ? new Recipe[0] : ExecuteSearch(SearchText);
            var recipeViewModel = _recipeMapper.Map(recipes).OrderBy(r => r.Name).ToArray();

            // Message
            var message = SearchText == null ? "Welcome" : "{0} recipes found".StringFormat(recipeViewModel.Length);

            // Create the view model
            var model = new RecipeIndexViewModel
                {
                    Categories = categoryViewModels,
                    SearchResults = recipeViewModel,
                    SearchResultMessage = message,
                    SearchText = SearchText
                };
            return model;
        }

        private SelectedRecipeViewModel EditRecipe(string recipeId, bool isNew)
        {
            var recipeModel = isNew ? GetNewRecipeViewModel(recipeId) : GetSelectedRecipeViewModel(recipeId);
            return recipeModel;
        }

        private SelectedRecipeViewModel GetNewRecipeViewModel(string recipeId)
        {
            // Categories
            var categories = _categoryRepository.Get();
            var categoryViewModels = _categoryMapper.Map(categories).OrderBy(c => c.Description).ToArray();

            // Create a new recipe
            var recipeModel = new RecipeViewModel { RecipeId = recipeId};

            return new SelectedRecipeViewModel
            {
                Categories = categoryViewModels,
                Recipe = recipeModel
            };
        }

        private SelectedRecipeViewModel GetSelectedRecipeViewModel(string recipeId)
        {
            // Categories
            var categories = _categoryRepository.Get();
            var categoryViewModels = _categoryMapper.Map(categories).OrderBy(c=>c.Description).ToArray();

            // Obtain the given recipe
            var recipe = _recipeRepository.Get(recipeId);
            var recipeModel = recipe != null ?_recipeMapper.Map(recipe) : new RecipeViewModel {RecipeId = recipeId};

            return new SelectedRecipeViewModel
                       {
                           Categories = categoryViewModels,
                           Recipe = recipeModel
                       };
        }

        private IEnumerable<Recipe> ExecuteSearch(string searchTerms)
        {
            var nullSafeCriteria = searchTerms.SafeToLower().SafeTrim();

            // If there was no criteria, then get all
            if (nullSafeCriteria.IsEmpty())
                return _recipeRepository.Get();

            // See if there were any properties specified
            var parsedSearch = nullSafeCriteria.Split('=');
            if (parsedSearch[0].SafeTrim() == "category" && parsedSearch.Length>=2)
                return _recipeRepository.Find(recipe => string.Equals(recipe.Category,parsedSearch[1],StringComparison.InvariantCultureIgnoreCase));
            
            // Perform the search
            return _recipeRepository.Find(recipe => 
                   recipe.Category.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Name.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Ingredients.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Directions.SafeToLower().SafeContains(nullSafeCriteria)
                || recipe.Source.SafeToLower().SafeContains(nullSafeCriteria))
                .OrderBy(r=>r.Name);
        }

        private void CropImage(RecipeImageViewModel viewModel, WebImage image, int imageSize)
        {
            // Crop the image
            var height = image.Height;
            var width = image.Width;

            image.Crop((int) viewModel.Top, (int) viewModel.Left, (int) (height - viewModel.Bottom),
                       (int) (width - viewModel.Right));
            image.Save();

            // Resize it
            if (image.Width > imageSize)
                image.Resize(imageSize, imageSize);
        }

        private void RefreshRecipeData()
        {
            _recipeRepository.Refresh();
            _categoryRepository.Refresh();
        }

        private void SaveTemporaryImage(RecipeImageViewModel recipeViewModel, WebImage image)
        {
            // Resize the image to a manageable size
            if (image.Width > 750)
                image.Resize(750, 750);

            // Save a tempory version
            var filename = Path.GetFileName(image.FileName);
            var tempFileName = "{0}_temp{1}".StringFormat(recipeViewModel.RecipeId, Path.GetExtension(filename));
            var tempFilePath = _tempImageProvider.Save(image, tempFileName);

            // Persit to S3
            recipeViewModel.ImageUrl = _amazonS3FileProvider.Save(tempFilePath, "DomusRecipeImages");

            // Update the view model for cropping
            recipeViewModel.Width = image.Width;
            recipeViewModel.Height = image.Height;
            recipeViewModel.Top = image.Height * 0.1;
            recipeViewModel.Left = image.Width * 0.9;
            recipeViewModel.Right = image.Width * 0.9;
            recipeViewModel.Bottom = image.Height * 0.9;
        }
    }
}
