using System;
using System.Collections.Generic;
using System.Linq;
using Domus.Web.UI.Controllers;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Domus.WebUI.Test.Home
{
    [TestFixture]
    public class HomeControllerTests
    {

        [Test]
        public void When_showing_the_index_then_the_index_view_is_shown()
        {
            // Arrange
            var controller = new HomeController(null);

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result,Is.Not.Null);
        } 

    }
}