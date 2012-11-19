using Domus.Web.UI.Controllers;
using Domus.Web.UI.Infrastructure.DependencyInjection;
using NUnit.Framework;
using Ninject;

namespace Domus.WebUI.Test.DependencyInjection
{
    [TestFixture]
    [Category( "Integration" )]
    public class When_configuring_the_application
    {
        private IKernel _kernel;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _kernel = new StandardKernel();
            Bootstrapper.Configure(_kernel);
        }


        [Test]
        public void Then_the_home_controller_can_be_resolved()
        {
            // Assert
            Assert.That(_kernel.Get<HomeController>(),Is.Not.Null);
        }

        [Test]
        public void Then_the_recipe_controller_can_be_resolved()
        {
            // Assert
            Assert.That(_kernel.Get<RecipeController>(),Is.Not.Null);
        }
    }
}