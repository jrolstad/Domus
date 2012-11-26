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
    public class When_saving_a_recipe_that_has_validation_errors
    {
        private ViewResult _viewResult;
        private SelectedRecipeViewModel _viewModel;
        private IList<Category> _categoriesFromProvider;
        private SelectedRecipeViewModel _viewModelToSave;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            
            // Creat an invalid recipe (Name is required)
            var recipeToSave = Builder<RecipeViewModel>.CreateNew().Build();

            _viewModelToSave = new SelectedRecipeViewModel {Recipe = recipeToSave};

            this._categoriesFromProvider = Builder<Category>.CreateListOfSize(20).Build();
            var categoryProvider = MockRepository.GenerateStub<IDataProvider<Category,string>>();
            categoryProvider.Stub(p => p.Get()).Return(this._categoriesFromProvider);

            var controller = new RecipeController(MockRepository.GenerateStrictMock<IDataProvider<Recipe, string>>(),
                                                  categoryProvider,
                                                  new AutoMapperAdapter<Recipe, RecipeViewModel>(),
                                                  new AutoMapperAdapter<RecipeViewModel, Recipe>(),
                                                  new AutoMapperAdapter<Category, CategoryViewModel>(),
                                                  MockRepository.GenerateStub<TempImageProvider>(),
                                                  MockRepository.GenerateStub<AmazonS3FileProvider>(),
                                                  MockRepository.GenerateStub<IFeatureUsageNotifier>()
                );
            controller.ViewData.ModelState.AddModelError("Name", "Bad Name");

            this._viewResult = controller.Save(_viewModelToSave) as ViewResult;
            if(_viewResult != null)
                this._viewModel = this._viewResult.Model as SelectedRecipeViewModel;
        }


        [Test]
        public void Then_the_view_model_is_shown()
        {
            // Assert
            Assert.That(this._viewModel,Is.Not.Null);
        }


        [Test]
        public void Then_the_categories_are_shown()
        {
            // Assert
            Assert.That(this._viewModel.Categories.Select(c=>c.Description).ToArray(),
                        Is.EquivalentTo(this._categoriesFromProvider.Select(c=>c.Description).ToArray()));
        }

        [Test]
        public void Then_the_search_terms_are_empty()
        {
            // Assert
            Assert.That(this._viewModel.SearchText, Is.Null);
        }

        [Test]
        public void Then_the_recipe_is_re_shown()
        {
            // Assert
            Assert.That(this._viewModel.Recipe, Is.SameAs(_viewModelToSave.Recipe));
            Assert.That(this._viewResult.ViewName,Is.EqualTo("Edit"));
        }

    }
}