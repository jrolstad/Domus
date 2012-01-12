using System;
using System.Collections.Generic;
using System.Linq;
using Domus.Adapters;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Infrastructure.DependencyInjection;
using Domus.Web.UI.Models.Recipes;
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
            Web.UI.Properties.Settings.Default["AmazonAccessKey"] = "access";
            Web.UI.Properties.Settings.Default["AmazonSecretKey"] = "secret";
            _kernel = new StandardKernel();
            Bootstrapper.Configure(_kernel);
        }


        [Test]
        public void Then_the_recipe_provider_is_configured()
        {
            // Assert
            Assert.That(_kernel.Get<IDataProvider<Recipe, string>>(),Is.Not.Null);
        }

        [Test]
        public void Then_the_category_provider_is_configured()
        {
            // Assert
            Assert.That(_kernel.Get<IDataProvider<Category, string>>(), Is.Not.Null);
        }

        [Test]
        public void Then_the_category_adapter_is_configured()
        {
            // Assert
            Assert.That(_kernel.Get<IAdapter<Category,CategoryViewModel>>(), Is.Not.Null);
        }

        [Test]
        public void Then_the_recipe_adapter_is_configured()
        {
            // Assert
            Assert.That(_kernel.Get<IAdapter<Recipe, RecipeViewModel>>(), Is.Not.Null);
        }

        [Test]
        public void Then_the_recipe_view_model_adapter_is_configured()
        {
            // Assert
            Assert.That(_kernel.Get<IAdapter<RecipeViewModel,Recipe>>(), Is.Not.Null);
        }
    }
}