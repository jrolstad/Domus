﻿using Domus.Providers;
using Domus.Web.UI.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.WebUI.Test.Home
{
    [TestFixture]
    public class HomeControllerTests
    {

        [Test]
        public void When_showing_the_index_then_the_index_view_is_shown()
        {
            // Arrange
            var controller = new HomeController(null, null, null, MockRepository.GenerateStub<IFeatureUsageNotifier>());

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result,Is.Not.Null);
        } 

    }
}