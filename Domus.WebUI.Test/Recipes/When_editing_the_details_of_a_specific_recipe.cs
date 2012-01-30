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
    public class When_editing_the_details_of_a_specific_recipe
    {
        private ViewResult _viewResult;
        private SelectedRecipeViewModel _viewModel;
        private IList<Category> _categoriesFromProvider;
        private Recipe _recipeFromProvider;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            
            // Set up dependencies for controller
            this._recipeFromProvider = Builder<Recipe>.CreateNew().Build();
            var recipeProvider = MockRepository.GenerateStub<IDataProvider<Recipe,string>>();
            recipeProvider.Stub(p => p.Get(this._recipeFromProvider.RecipeId)).Return(this._recipeFromProvider);

            this._categoriesFromProvider = Builder<Category>.CreateListOfSize(20).Build();
            var categoryProvider = MockRepository.GenerateStub<IDataProvider<Category,string>>();
            categoryProvider.Stub(p => p.Get()).Return(this._categoriesFromProvider);

            var controller = new RecipeController(recipeProvider,
                                                  categoryProvider,
                                                  new AutoMapperAdapter<Recipe, RecipeViewModel>(),
                                                  new AutoMapperAdapter<RecipeViewModel, Recipe>(),
                                                  new AutoMapperAdapter<Category, CategoryViewModel>(),
                                                  MockRepository.GenerateStub<TempImageProvider>(),
                                                  MockRepository.GenerateStub<AmazonS3FileProvider>()
                );


            this._viewResult = controller.Edit(this._recipeFromProvider.RecipeId);
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
        public void Then_the_recipe_is_shown()
        {
            // Assert
            Assert.That(this._viewModel.Recipe.RecipeId, Is.EqualTo(this._recipeFromProvider.RecipeId));
        }

    }
}