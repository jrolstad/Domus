using System.Configuration;
using Domus.Entities;
using Domus.Providers;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class DataProviderRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<ICacheProvider>().To<MemcacheCache>().InSingletonScope();

            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];

            Bind<IDataProvider<Recipe, string>>().To<AmazonSimpleDbRecipeProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            Bind<IDataProvider<User, string>>().To<AmazonSimpleDbUserProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            Bind<IDataProvider<Category, string>>().To<AmazonSimpleDbCategoryProvider>()
              .InSingletonScope()
              .WithConstructorArgument("accessKey", accessKey)
              .WithConstructorArgument("secretKey", secretKey);

            Bind<AmazonS3FileProvider>()
                .ToMethod(p => new AmazonS3FileProvider(accessKey, secretKey))
              .InSingletonScope();

            Bind<IFeatureUsageNotifier>()
                  .To<FeatureUsageNotifier>()
                  .InSingletonScope()
                  .WithConstructorArgument("accessKey", accessKey)
                  .WithConstructorArgument("secretKey", secretKey);
        }
    }
}