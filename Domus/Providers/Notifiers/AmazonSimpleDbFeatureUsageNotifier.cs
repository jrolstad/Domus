using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Directus.SimpleDb.Providers;
using Domus;
using Domus.Entities;
using Domus.Providers;

namespace Notifiers
{
    public class AmazonSimpleDbFeatureUsageNotifier : IFeatureUsageNotifier
    {
        private readonly SimpleDBProvider<FeatureUsage, string> _simpleDbProvider;
        private readonly IPrincipal _currentUser;

        internal AmazonSimpleDbFeatureUsageNotifier(SimpleDBProvider<FeatureUsage, string> simpleDbProvider, IPrincipal currentUser)
        {
            _simpleDbProvider = simpleDbProvider;
            _currentUser = currentUser;
        }

        public AmazonSimpleDbFeatureUsageNotifier(string accessKey, string secretKey, IPrincipal currentUser)
            : this(new SimpleDBProvider<FeatureUsage, string>(accessKey, secretKey),currentUser)
        {
           
        }

        public void Notify(Feature feature, DateTime? usedAt = null, string usedBy = null, string notes = null)
        {
            try
            {
                var resolvedUsedAt = usedAt.GetValueOrDefault(Clock.Now).ToUniversalTime();
                var resolvedUsedBy = usedBy ?? _currentUser.Identity.Name;
                var resolvedNotes = notes ?? "";

                var task = new Task(() => SaveFeatureUsage(feature, resolvedUsedAt, resolvedUsedBy, resolvedNotes));
                task.Start();
            }
            catch
            {
                // We want to suppress any errors since its ok if feature logging fails
            }
        }

        private void SaveFeatureUsage(Feature feature, DateTime usedAt, string usedBy, string notes)
        {
            var usage = new FeatureUsage
                {
                    FeatureUsageId = Guid.NewGuid().ToString(),
                    FeatureName = feature.ToString(),
                    UsedAt = usedAt,
                    UsedBy = usedBy,
                    Notes = notes
                };

            _simpleDbProvider.Save(new[] {usage});
        }
    }
}