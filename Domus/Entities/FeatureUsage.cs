using System;
using System.ComponentModel.DataAnnotations;
using Directus.SimpleDb.Attributes;

namespace Domus.Entities
{
    [Serializable]
    [DomainName("Domus_FeatureUsage")]
    public class FeatureUsage
    {
        [Key]
        public string FeatureUsageId { get; set; }

        public string FeatureName { get; set; }

        public DateTime UsedAt { get; set; }

        public string UsedBy { get; set; }

        public string Notes { get; set; }
    }
}