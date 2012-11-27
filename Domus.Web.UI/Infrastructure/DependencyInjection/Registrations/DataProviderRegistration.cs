using System.Configuration;
using Domus.Entities;
using Domus.Providers;
using Domus.Providers.Cacheing;
using Notifiers;
using Domus.Providers.FileProviders;
using Domus.Providers.Repositories;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class DataProviderRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<ICache>().To<MemcacheCache>().InSingletonScope();

            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];

            Bind<IRepository<Recipe, string>>().To<AmazonSimpleDbRecipeProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            Bind<IRepository<User, string>>().To<AmazonSimpleDbUserProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            Bind<IRepository<Category, string>>().To<AmazonSimpleDbCategoryProvider>()
              .InSingletonScope()
              .WithConstructorArgument("accessKey", accessKey)
              .WithConstructorArgument("secretKey", secretKey);

            Bind<IFileProvider>()
                .ToMethod(p => new AmazonS3FileProvider(accessKey, secretKey))
              .InSingletonScope();

            Bind<IImageProvider>()
                .To<TempImageProvider>();

            Bind<IFeatureUsageNotifier>()
                  .To<AmazonSimpleDbFeatureUsageNotifier>()
                  .InSingletonScope()
                  .WithConstructorArgument("accessKey", accessKey)
                  .WithConstructorArgument("secretKey", secretKey);
        }
    }
}