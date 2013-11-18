using System.Linq;
using System.Web.Mvc;
using Domus.Web.Models;

namespace Domus.Web.Controllers
{
    public class RecipeController : Controller
    {
        private CategoryApiController _categoryApiController = new CategoryApiController();

        public ViewResult Index()
        {

            var categories = _categoryApiController
                .Get()
                .ToList();

            var viewModel = new RecipeIndexViewModel {Categories = categories};

            return View(viewModel);
        }
    }
}
