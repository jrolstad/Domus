using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using Directus.SimpleDb.Providers;
using Domus.Entities;
using Domus.Providers;
using Notifiers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Domus.Test.Providers
{
    [TestFixture]
    public class FeatureUsageNotifierTests
    {

        [Test]
        [TestCase(Feature.RecipeEdit,"I'm editing")]
        [TestCase(Feature.RecipeEdit,null)]
        [TestCase(Feature.RecipeEdit,"")]
        public void When_notifying_of_feature_usage_then_the_details_are_saved(Feature feature, string notes)
        {
            // Arrange
            using (Clock.Freeze())
            {
                var savedUsages = new List<FeatureUsage>();
                var provider = MockRepository.GenerateStub<SimpleDBProvider<FeatureUsage, string>>();
                provider.Stub(p => p.Save(Arg<ICollection<FeatureUsage>>.Is.Anything))
                        .WhenCalled(arg => { savedUsages.AddRange(arg.Arguments[0] as ICollection<FeatureUsage>); });

                var identity = MockRepository.GenerateStub<IIdentity>();
                identity.Stub(i => i.Name).Return("who I am");
                var currentUser = MockRepository.GenerateStub<IPrincipal>();
                currentUser.Stub(c => c.Identity).Return(identity);

                var notifier = new AmazonSimpleDbFeatureUsageNotifier(provider, currentUser);

                // Act
                notifier.Notify(feature, notes: notes);

                Thread.Sleep(200);

                // Assert
                Assert.That(savedUsages.Count, Is.EqualTo(1));
                Assert.That(string.IsNullOrWhiteSpace(savedUsages[0].FeatureUsageId), Is.False);
                Assert.That(savedUsages[0].FeatureName, Is.EqualTo(feature.ToString()));
                Assert.That(savedUsages[0].UsedAt.ToString(), Is.EqualTo(Clock.Now.ToUniversalTime().ToString()));
                Assert.That(savedUsages[0].UsedBy, Is.EqualTo("who I am"));
                Assert.That(savedUsages[0].Notes, Is.EqualTo(notes ?? ""));
            }
        } 

    }
}