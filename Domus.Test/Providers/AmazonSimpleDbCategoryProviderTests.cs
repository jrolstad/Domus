using System;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;
using Domus.Providers;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class AmazonSimpleDbCategoryProviderTests
    {
        [Test]
        public void When_getting_a_Category_then_the_Category_is_obtained_from_simpledb()
        {
            // Arrange
            var CategoryId = Guid.NewGuid().ToString();

            var CategoryInDb = Builder<Category>.CreateNew().Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Category,string>>();
            simpleDB.Stub(sdb => sdb.Get(CategoryId)).Return(CategoryInDb);

            var provider = new AmazonSimpleDbCategoryProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Get(CategoryId);

            // Assert
            Assert.That(result,Is.EqualTo(CategoryInDb));
        }

        [Test]
        public void When_getting_all_Category_then_they_are_obtained_from_simpledb()
        {
            // Arrange

            var CategorysInDb = Builder<Category>.CreateListOfSize(10).Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Category, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(CategorysInDb);

            var provider = new AmazonSimpleDbCategoryProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Get();

            // Assert
            Assert.That(result, Is.EquivalentTo(CategorysInDb));
        }

        [Test]
        public void When_searching_for_Categorys_then_the_matches_are_obtained_from_simpledb()
        {
            // Arrange

            var CategorysInDb = Builder<Category>.CreateListOfSize(10).Build();
            CategorysInDb[2].Description = "Some Name";

            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Category, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(CategorysInDb);

            var provider = new AmazonSimpleDbCategoryProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            var result = provider.Search(r=>r.Description == "Some Name");

            // Assert
            Assert.That(result.Single().Description, Is.EqualTo("Some Name"));
        }

        [Test]
        public void When_saving_a_Category_then_the_Category_is_saved_to_simpledb()
        {
            // Arrange

            var Category = Builder<Category>.CreateNew().Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Category, string>>();

            var provider = new AmazonSimpleDbCategoryProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            provider.Save(Category);

            // Assert
            simpleDB.AssertWasCalled(db=>db.Save(new[]{Category}));
        }

        [Test]
        public void When_deleting_a_Category_then_the_Category_is_deleted_from_simpledb()
        {
            // Arrange
            var CategoryId = Guid.NewGuid().ToString();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<Category, string>>();

            var provider = new AmazonSimpleDbCategoryProvider(simpleDB, MockRepository.GenerateStub<ICacheProvider>());

            // Act
            provider.Delete(CategoryId);

            // Assert
            simpleDB.AssertWasCalled(db => db.Delete(new[] { CategoryId }));
        }
    }
}