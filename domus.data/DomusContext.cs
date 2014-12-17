using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using domus.data.models;

namespace domus.data
{
    public class DomusContext:DbContext
    {
        public DomusContext():base("DomusContext")
        {
            
        }
        public IDbSet<Recipe> Recipes { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public override int SaveChanges()
        {
            SetCreateValues("user",DateTime.Now);
            SetLastUpdateValues("user", DateTime.Now);

            return base.SaveChanges();
        }

        public IEnumerable<DbEntityEntry> GetEntries(EntityState entityState)
        {
            ChangeTracker.DetectChanges();

            var modifiedEntries = this.ChangeTracker.Entries()
                .Where(p => p.State == entityState);
            return modifiedEntries;
        }

        private void SetCreateValues(string submittedBy, DateTime submittedAt)
        {
            var addedEntries = this.GetEntries(EntityState.Added);
            foreach (var modifiedEntry in addedEntries)
            {
                var auditable = modifiedEntry.Entity as IAuditableModel;
                if (auditable == null)
                    continue;

                auditable.CreatedBy = submittedBy;
                auditable.CreatedAt = submittedAt;
                auditable.LastUpdatedBy = submittedBy;
                auditable.LastUpdatedAt = submittedAt;
            }
        }

        private void SetLastUpdateValues(string submittedBy, DateTime submittedAt)
        {
            var modifiedEntries = this.GetEntries(EntityState.Modified);
            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditable = modifiedEntry.Entity as IAuditableModel;
                if (auditable == null)
                    continue;

                auditable.LastUpdatedBy = submittedBy;
                auditable.LastUpdatedAt = submittedAt;
            }
        }
    }
}