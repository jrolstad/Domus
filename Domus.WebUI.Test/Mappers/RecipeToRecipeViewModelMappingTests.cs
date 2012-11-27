using Domus.Entities;
using Domus.Mappers;
using Domus.Web.UI.Models.Recipes;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace Domus.WebUI.Test.Mappers
{
    [TestFixture]
    public class RecipeToRecipeViewModelMappingTests
    {
        private Recipe _input;
        private RecipeViewModel _result;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _input = Builder<Recipe>.CreateNew().Build();

            var mapper = new AutoMapperMapper<Recipe, RecipeViewModel>();

            _result = mapper.Map(_input);

        }

        [Test]
        public void Then_RecipeId_is_mapped()
        {
            // Assert
            Assert.That(_result.RecipeId,Is.EqualTo(_input.RecipeId));
        }

        [Test]
        public void Then_Name_is_mapped()
        {
            // Assert
            Assert.That(_result.Name, Is.EqualTo(_input.Name));
        }

        [Test]
        public void Then_Servings_is_mapped()
        {
            // Assert
            Assert.That(_result.Servings, Is.EqualTo(_input.Servings));
        }

        [Test]
        public void Then_Rating_is_mapped()
        {
            // Assert
            Assert.That(_result.Rating, Is.EqualTo(_input.Rating));
        }

        [Test]
        public void Then_Category_is_mapped()
        {
            // Assert
            Assert.That(_result.Category, Is.EqualTo(_input.Category));
        }

        [Test]
        public void Then_PreviousCategory_is_mapped()
        {
            // Assert
            Assert.That(_result.PreviousCategory, Is.EqualTo(_input.PreviousCategory));
        }

        [Test]
        public void Then_Ingredients_is_mapped()
        {
            // Assert
            Assert.That(_result.Ingredients, Is.EqualTo(_input.Ingredients));
        }

        [Test]
        public void Then_Directions_is_mapped()
        {
            // Assert
            Assert.That(_result.Directions, Is.EqualTo(_input.Directions));
        }

        [Test]
        public void Then_Source_is_mapped()
        {
            // Assert
            Assert.That(_result.Source, Is.EqualTo(_input.Source));
        }

        [Test]
        public void Then_ImageUrl_is_mapped()
        {
            // Assert
            Assert.That(_result.ImageUrl, Is.EqualTo(_input.ImageUrl));
        }
    }
}