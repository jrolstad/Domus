using System;
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
    public class When_editing_a_new_recipe
    {
        private ViewResult _viewResult;
        private SelectedRecipeViewModel _viewModel;
        private IList<Category> _categoriesFromProvider;
        private string _newRecipeId;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _newRecipeId = Guid.NewGuid().ToString();

            // Set up dependencies for controller
            var recipeProvider = MockRepository.GenerateStub<IDataProvider<Recipe,string>>();

            this._categoriesFromProvider = Builder<Category>.CreateListOfSize(20).Build();
            var categoryProvider = MockRepository.GenerateStub<IDataProvider<Category,string>>();
            categoryProvider.Stub(p => p.Get()).Return(this._categoriesFromProvider);

            var controller = new RecipeController(recipeProvider,
                                                  categoryProvider,
                                                  new AutoMapperAdapter<Recipe, RecipeViewModel>(),
                                                  new AutoMapperAdapter<RecipeViewModel, Recipe>(),
                                                  new AutoMapperAdapter<Category, CategoryViewModel>()
                );


            this._viewResult = controller.Edit(_newRecipeId,true);
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
            Assert.That(this._viewModel.Recipe.RecipeId, Is.EqualTo(_newRecipeId));
        }

    }
}