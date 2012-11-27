using Domus.Adapters;
using Domus.Entities;
using Domus.Web.UI.Models.Recipes;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class AdapterRegistration:NinjectModule
    {
        public override void Load()
        {
            Bind<KeepAliveHandler>()
                .To<KeepAliveHandler>()
                .InSingletonScope()
                .OnDeactivation(context => context.Dispose());

           Bind<IAdapter<Recipe, RecipeViewModel>>()
                .To<AutoMapperAdapter<Recipe, RecipeViewModel>>()
                .InSingletonScope();

            Bind<IAdapter<RecipeViewModel, Recipe>>()
                .To<AutoMapperAdapter<RecipeViewModel, Recipe>>()
                .InSingletonScope();

            Bind<IAdapter<Category, CategoryViewModel>>()
                .To<AutoMapperAdapter<Category, CategoryViewModel>>()
                .InSingletonScope();
        }
    }
}