using Domus.Adapters;
using Domus.Entities;
using Domus.Web.UI.Models.Recipes;
using Ninject;
using Rolstad.DependencyInjection;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class AdapterRegistration:IContainerRegistration
    {
        public void Register( IKernel kernel )
        {
            kernel.Bind<KeepAliveHandler>()
                .To<KeepAliveHandler>()
                .InSingletonScope()
                .OnDeactivation(context => context.Dispose());

            kernel.Bind<IAdapter<Recipe, RecipeViewModel>>()
                .To<AutoMapperAdapter<Recipe, RecipeViewModel>>()
                .InSingletonScope();

            kernel.Bind<IAdapter<RecipeViewModel, Recipe>>()
                .To<AutoMapperAdapter<RecipeViewModel, Recipe>>()
                .InSingletonScope();

            kernel.Bind<IAdapter<Category, CategoryViewModel>>()
                .To<AutoMapperAdapter<Category, CategoryViewModel>>()
                .InSingletonScope();
        }
    }
}