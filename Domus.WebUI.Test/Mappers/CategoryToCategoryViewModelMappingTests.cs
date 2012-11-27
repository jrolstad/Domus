using Domus.Entities;
using Domus.Mappers;
using Domus.Web.UI.Models.Recipes;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace Domus.WebUI.Test.Mappers
{
    [TestFixture]
    public class CategoryToCategoryViewModelMappingTests
    {
        private Category _input;
        private CategoryViewModel _result;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _input = Builder<Category>.CreateNew().Build();

            var mapper = new AutoMapperMapper<Category, CategoryViewModel>();

            _result = mapper.Map(_input);

        }

        [Test]
        public void Then_Description_is_mapped()
        {
            // Assert
            Assert.That(_result.Description,Is.EqualTo(_input.Description));
        }

      
    }
}