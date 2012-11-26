using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domus.Adapters;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Controllers;
using Domus.Web.UI.Models.Recipes;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.WebUI.Test.Recipes
{
    [TestFixture]
    public class When_saving_a_valid_recipe
    {
        private RedirectToRouteResult _viewResult;
        private IList<Category> _categoriesFromProvider;
        private SelectedRecipeViewModel _viewModelToSave;
        private IDataProvider<Recipe, string> _recipeProvider;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            
            // Creat an invalid recipe (Name is required)
            var recipeToSave = Builder<RecipeViewModel>.CreateNew().Build();

            this._viewModelToSave = new SelectedRecipeViewModel {Recipe = recipeToSave};

            _recipeProvider = MockRepository.GenerateStub<IDataProvider<Recipe, string>>();

            this._categoriesFromProvider = Builder<Category>.CreateListOfSize(20).Build();
            var categoryProvider = MockRepository.GenerateStub<IDataProvider<Category,string>>();
            categoryProvider.Stub(p => p.Get()).Return(this._categoriesFromProvider);

            
            var controller = new RecipeController(_recipeProvider,
                                                  categoryProvider,
                                                  new AutoMapperAdapter<Recipe, RecipeViewModel>(),
                                                  new AutoMapperAdapter<RecipeViewModel, Recipe>(),
                                                  new AutoMapperAdapter<Category, CategoryViewModel>(),
                                                  MockRepository.GenerateStub<TempImageProvider>(),
                                                  MockRepository.GenerateStub<AmazonS3FileProvider>(),
                                                  MockRepository.GenerateStub<IFeatureUsageNotifier>()
                );

            this._viewResult = controller.Save(this._viewModelToSave) as RedirectToRouteResult;
           
        }


        [Test]
        public void Then_the_recipe_is_saved()
        {
            // Assert
            _recipeProvider.AssertWasCalled(p=>p.Save(Arg<Recipe>.Is.Anything));
        }

        [Test]
        public void Then_the_user_is_redirected_to_the_Recipe_details()
        {
            // Assert
            Assert.That(this._viewResult.RouteValues["action"], Is.EqualTo("Detail"));
            Assert.That(this._viewResult.RouteValues["recipeId"], Is.EqualTo(_viewModelToSave.Recipe.RecipeId));
        }

    }
}