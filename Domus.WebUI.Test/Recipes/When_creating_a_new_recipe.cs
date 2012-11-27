using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domus.Entities;
using Domus.Mappers;
using Domus.Providers;
using Domus.Web.UI.Controllers;
using Domus.Web.UI.Models.Recipes;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.WebUI.Test.Recipes
{
    [TestFixture]
    public class When_creating_a_new_recipe
    {
        private RedirectToRouteResult _viewResult;


        [TestFixtureSetUp]
        public void BeforeAll()
        {
            
            // Set up dependencies for controller
            var recipeProvider = MockRepository.GenerateStrictMock<IRepository<Recipe,string>>();
            var categoryProvider = MockRepository.GenerateStub<IRepository<Category,string>>();

            var controller = new RecipeController(recipeProvider,
                                                  categoryProvider,
                                                  new AutoMapperMapper<Recipe, RecipeViewModel>(),
                                                  new AutoMapperMapper<RecipeViewModel, Recipe>(),
                                                  new AutoMapperMapper<Category, CategoryViewModel>(),
                                                  MockRepository.GenerateStub<IImageProvider>(),
                                                  MockRepository.GenerateStub<IFileProvider>(),
                                                  MockRepository.GenerateStub<IFeatureUsageNotifier>()
                );

            this._viewResult = controller.Create();
        }


        [Test]
        public void Then_a_new_recipe_is_shown_in_the_editor()
        {
            // Assert
            Assert.That(this._viewResult.RouteValues["action"], Is.EqualTo("Edit"));
            Assert.That(this._viewResult.RouteValues["recipeId"],Is.Not.Null);
            Assert.That(this._viewResult.RouteValues["isNew"], Is.True);
        }


    }
}