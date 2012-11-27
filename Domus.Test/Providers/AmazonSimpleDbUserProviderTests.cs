using System;
using System.Linq;
using Directus.SimpleDb.Providers;
using Domus.Entities;
using Domus.Providers;
using Domus.Providers.Repositories;
using FizzWare.NBuilder;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class AmazonSimpleDbUserProviderTests
    {

        [Test]
        public void When_getting_a_User_then_the_User_is_obtained_from_simpledb()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var userInDb = Builder<User>.CreateNew().Build();
            var simpleDb = MockRepository.GenerateStub<SimpleDBProvider<User,string>>();
            simpleDb.Stub(sdb => sdb.Get(userId)).Return(userInDb);

            var provider = new AmazonSimpleDbUserProvider(simpleDb);

            // Act
            var result = provider.Get(userId);

            // Assert
            Assert.That(result,Is.EqualTo(userInDb));
        }

        [Test]
        public void When_getting_all_User_then_they_are_obtained_from_simpledb()
        {
            // Arrange

            var usersInDb = Builder<User>.CreateListOfSize(10).Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<User, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(usersInDb);

            var provider = new AmazonSimpleDbUserProvider(simpleDB);

            // Act
            var result = provider.Get();

            // Assert
            Assert.That(result, Is.EquivalentTo(usersInDb));
        }

        [Test]
        public void When_searching_for_Users_then_the_matches_are_obtained_from_simpledb()
        {
            // Arrange

            var usersInDb = Builder<User>.CreateListOfSize(10).Build();
            usersInDb[2].Email = "Some Name";

            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<User, string>>();
            simpleDB.Stub(sdb => sdb.Get()).Return(usersInDb);

            var provider = new AmazonSimpleDbUserProvider(simpleDB);

            // Act
            var result = provider.Find(r=>r.Email == "Some Name");

            // Assert
            Assert.That(result.Single().Email, Is.EqualTo("Some Name"));
        }

        [Test]
        public void When_saving_a_User_then_the_User_is_saved_to_simpledb()
        {
            // Arrange

            var User = Builder<User>.CreateNew().Build();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<User, string>>();

            var provider = new AmazonSimpleDbUserProvider(simpleDB);

            // Act
            provider.Save(User);

            // Assert
            simpleDB.AssertWasCalled(db=>db.Save(new[]{User}));
        }

        [Test]
        public void When_deleting_a_User_then_the_User_is_deleted_from_simpledb()
        {
            // Arrange
            var UserId = Guid.NewGuid().ToString();
            var simpleDB = MockRepository.GenerateStub<SimpleDBProvider<User, string>>();

            var provider = new AmazonSimpleDbUserProvider(simpleDB);

            // Act
            provider.Delete(UserId);

            // Assert
            simpleDB.AssertWasCalled(db => db.Delete(new[] { UserId }));
        }
    }
}