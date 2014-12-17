using System.Data.Entity;
using domus.data;

namespace domus.mvc
{
    public class DatabaseConfig
    {
        public void ConfigureDatabase()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DomusContext>());
        }
    }
}