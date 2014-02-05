using System;
using System.Linq;
using System.Web.Mvc;
using Domus.Web.Models;

namespace Domus.Web.Controllers
{
    public class RecipeController : Controller
    {
        private CategoryApiController _categoryApiController = new CategoryApiController();

        public ViewResult Index(string category, string searchTerms)
        {
            var viewModel = BuildIndexPage();

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
           return RedirectToAction("Index");
        }
    }
}
