using System;
using System.Threading.Tasks;
using System.Web;
using Directus.SimpleDb.Providers;
using Domus.Entities;

namespace Domus.Providers
{
    public class FeatureUsageNotifier : IFeatureUsageNotifier
    {
        private readonly SimpleDBProvider<FeatureUsage, string> _simpleDbProvider;

        internal FeatureUsageNotifier(SimpleDBProvider<FeatureUsage, string> simpleDbProvider)
        {
            _simpleDbProvider = simpleDbProvider;
        }

        public FeatureUsageNotifier(string accessKey, string secretKey)
            : this(new SimpleDBProvider<FeatureUsage, string>(accessKey, secretKey))
        {
           
        }

        public void Notify(Feature feature, DateTime? usedAt = null, string usedBy = null, string notes = null)
        {
            try
            {
                var resolvedUsedAt = usedAt.GetValueOrDefault(DateTime.Now).ToUniversalTime();
                var resolvedUsedBy = ResolveUserName(usedBy);
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

        private static string ResolveUserName(string usedBy)
        {
            var resolvedUserName = usedBy ?? "";

            if (string.IsNullOrWhiteSpace(usedBy))
            {
                if (HttpContext.Current != null
                    && HttpContext.Current.User != null)
                {
                    resolvedUserName = HttpContext.Current.User.Identity.Name;
                }

            }

            return resolvedUserName;
        }
    }
}