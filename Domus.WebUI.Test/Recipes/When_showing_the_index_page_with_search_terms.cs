using System;
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
    public class When_showing_the_index_page_with_search_terms
    {
        private ViewResult _viewResult;
        private RecipeIndexViewModel _viewModel;
        private IList<Category> _categoriesFromProvider;
        private IList<Recipe> _recipesFromProvider;
        private string _searchTerms;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            
            // Set up dependencies for controller
            this._recipesFromProvider = Builder<Recipe>.CreateListOfSize(10).Build();
            var recipeProvider = MockRepository.GenerateStub<IRepository<Recipe,string>>();
            recipeProvider.Stub(p => p.Find(Arg<Func<Recipe, bool>>.Is.Anything)).Return(this._recipesFromProvider);

            _categoriesFromProvider = Builder<Category>.CreateListOfSize(20).Build();
            var categoryProvider = MockRepository.GenerateStub<IRepository<Category,string>>();
            categoryProvider.Stub(p => p.Get()).Return(_categoriesFromProvider);

            var controller = new RecipeController(recipeProvider,
                                                  categoryProvider,
                                                  new AutoMapperMapper<Recipe, RecipeViewModel>(),
                                                  new AutoMapperMapper<RecipeViewModel, Recipe>(),
                                                  new AutoMapperMapper<Category, CategoryViewModel>(),
                                                  MockRepository.GenerateStub<IImageProvider>(),
                                                  MockRepository.GenerateStub<IFileProvider>(),
                                                  MockRepository.GenerateStub<IFeatureUsageNotifier>()
                );

            this._searchTerms = "find me";

            _viewResult = controller.Index(this._searchTerms);
            _viewModel = _viewResult.Model as RecipeIndexViewModel;
        }


        [Test]
        public void Then_the_view_model_is_shown()
        {
            // Assert
            Assert.That(_viewModel,Is.Not.Null);
        }


        [Test]
        public void Then_the_categories_are_shown()
        {
            // Assert
            Assert.That(_viewModel.Categories.Select(c=>c.Description).ToArray(),
                Is.EquivalentTo(_categoriesFromProvider.Select(c=>c.Description).ToArray()));
        }

        [Test]
        public void Then_the_search_terms_are_shown()
        {
            // Assert
            Assert.That(_viewModel.SearchText, Is.EquivalentTo(_searchTerms));
        }

        [Test]
        public void Then_the_search_results_are_shown()
        {
            // Assert
            Assert.That(_viewModel.SearchResults.Select(r => r.Name).ToArray(), 
                Is.EquivalentTo(_recipesFromProvider.Select(r => r.Name).ToArray()));
        }

    }
}