using Domus.Web.UI.Infrastructure.DependencyInjection.Registrations;
using Ninject;
using Rolstad.DependencyInjection;

namespace Domus.Web.UI.Infrastructure.DependencyInjection
{
    public static class Bootstrapper
    {
         public static void Configure(IKernel kernel)
         {
             IoC.SetKernel(kernel);
             IoC.Configure(new IContainerRegistration[]
                               {
                                   new AdapterRegistration(), 
                                   new DataProviderRegistration(), 
                               }
                 );
         }
    }
}