using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using domus.data;
using domus.mvc.Models.Categories;
using domus.mvc.Models.Recipes;

namespace domus.mvc.Controllers
{
    public class RecipeController : Controller
    {
        private readonly DomusContext _dbContext;

        public RecipeController():this(new DomusContext())
        {
            
        }
        public RecipeController(DomusContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ViewResult Index()
        {
            var categories = _dbContext
                .Categories
                .ToList()
                .Select(c => new CategoryViewModel {Id = c.Id, Name = c.Name,SearchUrl = Url.Action("Search",new{searchTerms=string.Format("Category={0}",c.Name)})})
                .ToList();

            var viewModel = new RecipeSearchResultViewModel {Categories = categories};

            return View(viewModel);
        }
        public ActionResult Search(RecipeSearchRequest request)
        {
            return Index();
        }
    }
}