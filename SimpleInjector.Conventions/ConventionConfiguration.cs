using System;
using System.Reflection;

namespace SimpleInjector
{
    public delegate bool AssemblyFilter(Assembly assembly);
    public delegate bool Convention(Type service, Type type);

    public class ConventionConfiguration
    {
        // Properties
        public Lifestyle Lifestyle { get; set; }
        public AssemblyFilter AssemblyFilter { get; set; }
        public Convention ImplementationFilter { get; set; }
        public Convention DecoratorFilter { get; set; }


        // C'tor
        public ConventionConfiguration()
        {
            this.Lifestyle = Lifestyle.Transient;
            this.AssemblyFilter = DefaultAssemblyFilter;
            this.ImplementationFilter = DefaultImplementationFilter;
            this.DecoratorFilter = DefaultDecoratorFilter;
        }


        // Private Members
        private bool DefaultAssemblyFilter(Assembly assembly)
        {
            return true;
        }

        private bool DefaultImplementationFilter(Type service, Type implementation)
        {
            return implementation.Name.EndsWith(service.Name.Substring(1));
        }

        private bool DefaultDecoratorFilter(Type service, Type decorator)
        {
            return decorator.Name.EndsWith(service.Name.Substring(1) + "Decorator");
        }
    }
}