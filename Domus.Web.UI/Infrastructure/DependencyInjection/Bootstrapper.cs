﻿using Domus.Web.UI.Infrastructure.DependencyInjection.Registrations;
using Ninject;
using Ninject.Modules;

namespace Domus.Web.UI.Infrastructure.DependencyInjection
{
    public static class Bootstrapper
    {
        private static IKernel _kernel;
         public static void Configure(IKernel kernel)
         {
             var modulesToLoad = new NinjectModule[]
                 {
                     new MapperRegistration(),
                     new DataProviderRegistration(),
                     new AuthenticationRegistration(), 
                     new CommandRegistration(), 
                 };
             kernel.Load(modulesToLoad);

             _kernel = kernel;
         }

        public static IKernel GetKernel()
        {
            return _kernel;
        }
    }
}