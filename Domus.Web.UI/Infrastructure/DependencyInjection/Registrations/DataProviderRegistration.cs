using Domus.Entities;
using Domus.Providers;
using Domus.Providers.Amazon;
using Ninject;
using Rolstad.DependencyInjection;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class DataProviderRegistration : IContainerRegistration
    {
        public void Register(IKernel kernel)
        {
            kernel.Bind<IDataProvider<Recipe, string>>()
                .ToMethod(
                    delegate
                        {
                            return new AmazonSimpleDbRecipeDataProvider(Properties.Settings.Default.AmazonAccessKey, Properties.Settings.Default.AmazonSecretKey);
                        }
                );
            kernel.Bind<IDataProvider<Category, string>>().To<StaticCategoryDataProvider>();
        }
    }
}