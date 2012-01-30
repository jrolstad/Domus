using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Domus.Adapters;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Models.Recipes;
using Rolstad.Extensions;
using Rolstad.MVC.Errors;
using log4net;

namespace Domus.Web.UI.Controllers
{
    /// <summary>
    /// Controller for recipe data management
    /// </summary>
    [HandleErrorAndLog]
    public class RecipeController : Controller
    {
        /// <summary>
        /// Used for logging messages
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof (RecipeController));

        private readonly IDataProvider<Recipe, string> _recipeDataProvider;
        private readonly IDataProvider<Category, string> _categoryDataProvider;
        private readonly IAdapter<Recipe, RecipeViewModel> _recipeAdapter;
        private readonly IAdapter<RecipeViewModel, Recipe> _recipeViewModelAdapter;
        private readonly IAdapter<Category, CategoryViewModel> _categoryAdapter;
        private readonly TempImageProvider _tempImageProvider;
        private readonly AmazonS3FileProvider _amazonS3FileProvider;

        /// <summary>
        /// Constructor with all dependencies
        /// </summary>
        /// <param name="recipeDataProvider">Provider for recipe data</param>
        /// <param name="categoryDataProvider">Provder for categories</param>
        /// <param name="recipeAdapter">Adapter for converting from the recipe domain model to view model</param>
        /// <param name="recipeViewModelAdapter">Adapter for converting from the recipe view model to domain model</param>
        /// <param name="categoryAdapter">Adapter for converting from the category domain model to view model</param>
        /// <param name="tempImageProvider">Provider for persisting temporary images to disk</param>
        /// <param name="amazonS3FileProvider">Provider for persisting files to Amazon S3 </param>
        public RecipeController(IDataProvider<Recipe,string> recipeDataProvider,
                                IDataProvider<Category,string> categoryDataProvider,
                                IAdapter<Recipe,RecipeViewModel> recipeAdapter,
                                IAdapter<RecipeViewModel,Recipe> recipeViewModelAdapter,
                                IAdapter<Category,CategoryViewModel> categoryAdapter,
                                TempImageProvider tempImageProvider,
                                AmazonS3FileProvider amazonS3FileProvider
            )
        {
            _recipeDataProvider = recipeDataProvider;
            _categoryDataProvider = categoryDataProvider;
            _recipeAdapter = recipeAdapter;
            _recipeViewModelAdapter = recipeViewModelAdapter;
            _categoryAdapter = categoryAdapter;
            _tempImageProvider = tempImageProvider;
            _amazonS3FileProvider = amazonS3FileProvider;
        }

        /// <summary>
        /// Shows the index page
        /// </summary>
        /// <param name="SearchText">Possible text to search with</param>
        /// <returns></returns>
        public ViewResult Index(string SearchText)
        {
            if(Logger.IsInfoEnabled) Logger.InfoFormat("Executing search for '{0}'",SearchText ?? "null");

            // Categories
            var categories = _categoryDataProvider.Get();
            var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c=>c.Description).ToArray();

            // Recipes
            var recipes = SearchText == null ? new Recipe[0] : ExecuteSearch(SearchText);
            var recipeViewModel = _recipeAdapter.Convert(recipes).OrderBy(r=>r.Name).ToArray();

            // Message
            var message = SearchText == null ? "Welcome" : "{0} recipes found".StringFormat(recipeViewModel.Length);
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Message shown: '{0}'", message);

            // Create the view model
            var model = new RecipeIndexViewModel
                            {
                                Categories = categoryViewModels,
                                SearchResults = recipeViewModel,
                                SearchResultMessage = message,
                                SearchText = SearchText
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
            if(Logger.IsInfoEnabled) Logger.InfoFormat("Viewing details for recipe '{0}'",recipeId);

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
        [Authorize]
        public ViewResult Edit(string recipeId, bool isNew=false)
        {
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Editing details for recipe '{0}'", recipeId);

            // Obtain either a new recipe or get the existing one
            var recipeModel = isNew ? GetNewRecipeViewModel(recipeId) : GetSelectedRecipeViewModel(recipeId);

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

            if(Logger.IsInfoEnabled) Logger.InfoFormat("Saving changes to recipe '{0}'",selectedRecipe.Recipe.RecipeId);

            var recipe = _recipeViewModelAdapter.Convert(selectedRecipe.Recipe);
            _recipeDataProvider.Save(recipe);

            return RedirectToAction("Detail",new{recipeId = recipe.RecipeId});
        }

        /// <summary>
        /// Creates a new recipe and forwards for user input
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public RedirectToRouteResult Create()
        {
            if (Logger.IsInfoEnabled) Logger.Info("Creating a new recipe");

            return RedirectToAction("Edit", new { recipeId = Guid.NewGuid().ToString(), isNew = true});
        }

        /// <summary>
        /// Forces a refresh of cached data
        /// </summary>
        /// <returns></returns>
        public RedirectToRouteResult Refresh()
        {
            if (Logger.IsInfoEnabled) Logger.Info("Refreshing recipes");

            _recipeDataProvider.Refresh();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Creates the view model for a new recipe
        /// </summary>
        /// <returns></returns>
        internal SelectedRecipeViewModel GetNewRecipeViewModel(string recipeId)
        {
            // Categories
            var categories = _categoryDataProvider.Get();
            var categoryViewModels = _categoryAdapter.Convert(categories).OrderBy(c => c.Description).ToArray();

            // Create a new recipe
            var recipeModel = new RecipeViewModel { RecipeId = recipeId};

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
                || recipe.Source.SafeToLower().SafeContains(nullSafeCriteria))
                .OrderBy(r=>r.Name);
        }

        /// <summary>
        /// Given a recipe, allows an image to be uploaded for it and then cropped
        /// </summary>
        /// <param name="recipeViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult AddImage(RecipeImageViewModel recipeViewModel)
        {
            // Get the image
            var image = WebImage.GetImageFromRequest();

            // If there isn't one, re-show the details
            if (image == null)
                return RedirectToAction("Detail", new {recipeId = recipeViewModel.RecipeId});

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
            recipeViewModel.Top = image.Height*0.1;
            recipeViewModel.Left = image.Width*0.9;
            recipeViewModel.Right = image.Width*0.9;
            recipeViewModel.Bottom = image.Height*0.9;

            // Once saved, send so we can crop the image
            return View("ImageCrop", recipeViewModel);
        }

        /// <summary>
        /// Given a recipe, facilitate editing the related image
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="name"> </param>
        /// <returns></returns>
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
            var recipe = _recipeDataProvider.Get(viewModel.RecipeId);
            recipe.ImageUrl = recipeImageUrl;
            _recipeDataProvider.Save(recipe);

            // Delete temporary files
            _tempImageProvider.Delete(recipeImageFileName);
            _tempImageProvider.Delete(tempFileName);

            return RedirectToAction("Detail", new {recipeId = viewModel.RecipeId});
        }

        private static void CropImage(RecipeImageViewModel viewModel, WebImage image, int imageSize)
        {
            var height = image.Height;
            var width = image.Width;

            image.Crop((int) viewModel.Top, (int) viewModel.Left, (int) (height - viewModel.Bottom),
                       (int) (width - viewModel.Right));
            image.Save();

            if (image.Width > imageSize)
                image.Resize(imageSize, imageSize);
        }
    }
}
