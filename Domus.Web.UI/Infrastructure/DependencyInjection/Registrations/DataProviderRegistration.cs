using System.Configuration;
using Domus.Entities;
using Domus.Providers;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Ninject;
using Rolstad.DependencyInjection;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class DataProviderRegistration : IContainerRegistration
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<ICacheProvider>().To<MemcacheCache>().InSingletonScope();

            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];

            kernel.Bind<IDataProvider<Recipe, string>>().To<AmazonSimpleDbRecipeProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            kernel.Bind<IDataProvider<User, string>>().To<AmazonSimpleDbUserProvider>()
                .InSingletonScope()
                .WithConstructorArgument("accessKey", accessKey)
                .WithConstructorArgument("secretKey", secretKey);

            kernel.Bind<IDataProvider<Category, string>>().To<AmazonSimpleDbCategoryProvider>()
              .InSingletonScope()
              .WithConstructorArgument("accessKey", accessKey)
              .WithConstructorArgument("secretKey", secretKey);

            kernel.Bind<AmazonS3FileProvider>()
                .ToMethod(p => new AmazonS3FileProvider(accessKey, secretKey))
              .InSingletonScope();

            kernel.Bind<IFeatureUsageNotifier>()
                  .To<FeatureUsageNotifier>()
                  .InSingletonScope()
                  .WithConstructorArgument("accessKey", accessKey)
                  .WithConstructorArgument("secretKey", secretKey);
        }
    }
}