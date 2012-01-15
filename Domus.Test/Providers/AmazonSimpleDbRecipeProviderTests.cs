using System;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;
using Domus.Providers;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class AmazonSimpleDbRecipeProviderTests
    {
        [Test]
        public void When_creating_a_provider_with_credentials_then_the_underlying_provider_is_set()
        {
            // Arrange
            
            // Act
            var result = new AmazonSimpleDbRecipeProvider("access", "key", MockRepository.GenerateStub<ICacheProvider>());

            // Assert
            Assert.That(result._provider,Is.Not.Null);
        }

        [Test]
        public void When_getting_a_recipe_then_the_recipe_is_obtained_from_simpledb()
        {
            // Arrange
            var recipeId = Guid.NewGuid().ToString();

            var recipeInDb = Builder<Recipe>.CreateNew().Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Recipe,string>>();
            simpleDB.Stub(sdb => sdb.Get(recipeId)).Return(recipeInDb);

            var provider = new AmazonSimpleDbRecipeProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Get(recipeId);

            // Assert
            Assert.That(result,Is.EqualTo(recipeInDb));
        }

        [Test]
        public void When_getting_all_recipe_then_they_are_obtained_from_simpledb()
        {
            // Arrange

            var recipesInDb = Builder<Recipe>.CreateListOfSize(10).Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Recipe, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(recipesInDb);

            var provider = new AmazonSimpleDbRecipeProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Get();

            // Assert
            Assert.That(result, Is.EquivalentTo(recipesInDb));
        }

        [Test]
        public void When_searching_for_recipes_then_the_matches_are_obtained_from_simpledb()
        {
            // Arrange

            var recipesInDb = Builder<Recipe>.CreateListOfSize(10).Build();
            recipesInDb[2].Name = "Some Name";

            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Recipe, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(recipesInDb);

            var provider = new AmazonSimpleDbRecipeProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Search(r=>r.Name == "Some Name");

            // Assert
            Assert.That(result.Single().Name, Is.EqualTo("Some Name"));
        }

        [Test]
        public void When_saving_a_recipe_then_the_recipe_is_saved_to_simpledb()
        {
            // Arrange

            var recipe = Builder<Recipe>.CreateNew().Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Recipe, string>>();

            var provider = new AmazonSimpleDbRecipeProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            provider.Save(recipe);

            // Assert
            simpleDB.AssertWasCalled(db=>db.Save(new[]{recipe}));
        }

        [Test]
        public void When_deleting_a_recipe_then_the_recipe_is_deleted_from_simpledb()
        {
            // Arrange
            var recipeId = Guid.NewGuid().ToString();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Recipe, string>>();

            var provider = new AmazonSimpleDbRecipeProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            provider.Delete(recipeId);

            // Assert
            simpleDB.AssertWasCalled(db => db.Delete(new[] { recipeId }));
        }
    }
}