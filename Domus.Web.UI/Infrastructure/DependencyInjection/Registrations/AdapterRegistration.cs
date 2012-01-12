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
            kernel.Bind<IAdapter<Recipe, RecipeViewModel>>().To<AutoMapperAdapter<Recipe, RecipeViewModel>>();
            kernel.Bind<IAdapter<RecipeViewModel, Recipe>>().To<AutoMapperAdapter<RecipeViewModel, Recipe>>();

            kernel.Bind<IAdapter<Category, CategoryViewModel>>().To<AutoMapperAdapter<Category, CategoryViewModel>>();
        }
    }
}