using System;
using System.Security.Principal;
using System.Web;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection.Registrations
{
    public class AuthenticationRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<IPrincipal>()
              .ToMethod(context =>
              {
                  // Get the web user
                  if (HttpContext.Current != null)
                  {
                      var identity = HttpContext.Current.User.Identity;
                      return new GenericPrincipal(identity, new string[0]);
                  }

                  return new GenericPrincipal(new GenericIdentity(""),new string[0] );
              });
        }
    }
}