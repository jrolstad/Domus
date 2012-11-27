using Domus.Entities;
using Domus.Mappers;
using Domus.Web.UI.Models.Recipes;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class MapperRegistration:NinjectModule
    {
        public override void Load()
        {

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