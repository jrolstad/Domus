using System;
using System.Collections.Generic;
using System.Linq;
using Domus.Providers;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class StaticCategoryDataProviderTests
    {
        private StaticCategoryDataProvider _provider;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _provider = new StaticCategoryDataProvider();

        }

        [Test]
        public void When_obtaining_all_categories_then_there_are_some_obtained()
        {
            // Act
            var result = _provider.Get();

            // Assert
            Assert.That(result.Count(),Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void When_obtaining_all_categories_then_each_category_has_a_description()
        {
            // Act
            var result = _provider.Get();

            // Assert
            var descriptions = result.Select(r => r.Description).Distinct().ToArray();
            Assert.That(descriptions, Has.No.Member(null));
        }

        [Test]
        public void When_obtaining_a_specific_category_then_it_is_obtained()
        {
            // Arrange
            var allCategories = _provider.Get();
            var identifier = allCategories.First().Description;

            // Act
            var result = _provider.Get(identifier);

            // Assert
            Assert.That(result.Description,Is.EqualTo(identifier));
        }

        [Test]
        public void When_searching_then_only_matches_are_returned()
        {
            // Arrange
            var allCategories = _provider.Get();
            var categoriesThatStartWithC = allCategories
                .Where(c => c.Description.StartsWith("C", StringComparison.InvariantCultureIgnoreCase))
                .Select(c=>c.Description)
                .ToArray();

            // Act
            var result = _provider
                .Search(c => c.Description.StartsWith("C", StringComparison.InvariantCultureIgnoreCase))
                .Select(c => c.Description)
                .ToArray();

            // Assert
            Assert.That(result,Is.EquivalentTo(categoriesThatStartWithC));
        }
    }
}