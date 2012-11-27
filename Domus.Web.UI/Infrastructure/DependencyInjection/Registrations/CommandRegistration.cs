using Domus.Commands;
using Domus.Web.UI.Commands;
using Domus.Web.UI.Models.Home;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class CommandRegistration:NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand<Request, ApplicationDetailsResponse>>().To<ApplicationDetailsCommand>();
        }
    }
}