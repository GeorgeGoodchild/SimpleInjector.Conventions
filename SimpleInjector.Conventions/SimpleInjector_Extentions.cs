using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleInjector
{
    public static class SimpleInjector_Extentions
    {
        // Public Extensions
        public static void RegisterByConvention(this Container container)
        {
            container.RegisterByConvention(_ => { });
        }

        public static void RegisterByConvention(this Container container, Action<ConventionConfiguration> configureConventions)
        {
            var conventions = new ConventionConfiguration();
            configureConventions(conventions);

            RegisterByConvention(container, conventions.AssemblyFilter, conventions.ImplementationFilter, conventions.DecoratorFilter, conventions.Lifestyle);
        }


        // Private Helpers
        private static void RegisterByConvention(Container container, AssemblyFilter assemblyFilter, Convention implementationFilter, Convention decoratorFilter, Lifestyle lifestyle)
        {
            var types = GetAllLoadedTypes(assemblyFilter);

            var registrations = from service in types
                                let implementations = from implementation in types
                                                      where implementation.IsConcreteTypeThatImplements(service)
                                                         && implementationFilter(service, implementation)
                                                      select implementation
                                let decorators = from decorator in types
                                                 where decorator.IsConcreteTypeThatImplements(service)
                                                    && decoratorFilter(service, decorator)
                                                 select decorator
                                where service.IsInterface
                                   && !container.HasAlreadyRegistered(service)
                                select new
                                {
                                    Service = service,
                                    Implementations = implementations,
                                    Decorators = decorators
                                };

            foreach (var registration in registrations)
            {
                if (registration.Implementations.Count() == 1)
                {
                    container.Register(registration.Service, registration.Implementations.First(), lifestyle);
                }
                else if (registration.Implementations.Count() > 1)
                {
                    container.RegisterCollection(registration.Service, registration.Implementations);
                }

                foreach (var decorator in registration.Decorators)
                {
                    container.RegisterDecorator(registration.Service, decorator);
                }
            }
        }

        private static IEnumerable<Type> GetAllLoadedTypes(AssemblyFilter assemblyFilter = null)
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(x => assemblyFilter(x))
                            .SelectMany(x => x.GetExportedTypes());
        }

        private static bool IsConcreteTypeThatImplements(this Type instance, Type @interface)
        {
            return instance.GetInterfaces().Contains(@interface)
                && !instance.IsAbstract
                && !instance.IsInterface;
        }

        private static bool HasAlreadyRegistered(this Container container, Type serviceType)
        {
            return container.GetCurrentRegistrations()
                            .Any(x => x.ServiceType == serviceType);
        }
    }
}