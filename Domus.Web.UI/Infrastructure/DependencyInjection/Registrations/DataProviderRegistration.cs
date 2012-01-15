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
            kernel.Bind<IDataProvider<Recipe, string>>()
                .ToMethod(
                    delegate
                        {
                            return new AmazonSimpleDbRecipeProvider(Properties.Settings.Default.AmazonAccessKey, Properties.Settings.Default.AmazonSecretKey,kernel.Get<ICacheProvider>());
                        }
                ).InSingletonScope();
            kernel.Bind<IDataProvider<Category, string>>().To<StaticCategoryDataProvider>();
        }
    }
}