using System;
using Domus.Entities;

namespace Domus.Providers
{
    public interface IFeatureUsageNotifier
    {
        void Notify(Feature feature, DateTime? usedAt = null, string usedBy = null, string notes = null);
    }
}