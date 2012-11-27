using System;
using System.Configuration;
using System.Globalization;
using Domus.Commands;
using Domus.Providers.Repositories;
using Domus.Web.UI;
using Domus.Web.UI.Commands;
using Domus.Web.UI.Models.Home;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.WebUI.Test.Commands
{
    [TestFixture]
    public class When_getting_application_details
    {
        private ApplicationDetailsResponse _result;
        private DateTime _applicationStartTime;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            Clock.Freeze();
            _applicationStartTime = Clock.Now.AddHours(-5);

            var keepAliveHandler = MockRepository.GenerateStub<IKeepAliveHandler>();
            keepAliveHandler.ApplicationStartTime = _applicationStartTime;

            var command = new ApplicationDetailsCommand(keepAliveHandler);

            _result = command.Execute(Request.Empty);
        }

        [TestFixtureTearDown]
        public void AfterAll()
        {
            Clock.Thaw();
        }

        [Test]
        public void Then_the_AwsAccessKey_is_set()
        {
            // Assert
            Assert.That(_result.AwsAccessKey, Is.EqualTo(ConfigurationManager.AppSettings["AWS_ACCESS_KEY"]));
        }

        [Test]
        public void Then_the_AwsSecretKey_is_set()
        {
            // Assert
            Assert.That(_result.AwsSecretKey, Is.EqualTo(ConfigurationManager.AppSettings["AWS_SECRET_KEY"]));
        }

        [Test]
        public void Then_the_cache_duration_is_set()
        {
            // Assert
            Assert.That(_result.CacheDuration, Is.EqualTo(AmazonSimpleDbRecipeProvider.CacheDuration));
        }

        [Test]
        public void Then_the_ApplicationStartTime_is_set()
        {
            // Assert
            Assert.That(_result.ApplicationStartTime, Is.EqualTo(_applicationStartTime.ToString(CultureInfo.CurrentUICulture)));
        }

        [Test]
        public void Then_the_CurrentTime_is_set()
        {
            // Assert
            Assert.That(_result.CurrentTime, Is.EqualTo(Clock.Now.ToString(CultureInfo.CurrentUICulture)));
        }

        [Test]
        public void Then_the_ApplicationUpTime_is_set()
        {
            // Assert
            Assert.That(_result.ApplicationUpTime, Is.EqualTo(Clock.Now.Subtract(_applicationStartTime)));
        }

        [Test]
        public void Then_the_ServerName_is_set()
        {
            // Assert
            Assert.That(_result.ServerName,Is.EqualTo(Environment.MachineName));
        }

        [Test]
        public void Then_the_UserName_is_set()
        {
            // Assert
            Assert.That(_result.UserName, Is.EqualTo(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName)));
        }
    }
}