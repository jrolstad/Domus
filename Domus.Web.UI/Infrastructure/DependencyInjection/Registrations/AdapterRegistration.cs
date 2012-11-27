using Domus.Entities;
using Domus.Mappers;
using Domus.Web.UI.Models.Recipes;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class AdapterRegistration:NinjectModule
    {
        public override void Load()
        {
            Bind<IKeepAliveHandler>()
                .To<KeepAliveHandler>()
                .InSingletonScope()
                .OnDeactivation(context => context.Dispose());

           Bind<IMapper<Recipe, RecipeViewModel>>()
                .To<AutoMapperMapper<Recipe, RecipeViewModel>>()
                .InSingletonScope();

            Bind<IMapper<RecipeViewModel, Recipe>>()
                .To<AutoMapperMapper<RecipeViewModel, Recipe>>()
                .InSingletonScope();

            Bind<IMapper<Category, CategoryViewModel>>()
                .To<AutoMapperMapper<Category, CategoryViewModel>>()
                .InSingletonScope();
        }
    }
}