using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInjector.Conventions.ConsoleApplication
{
    public interface IFoo
    {
        void DoFoo();
    }

    public interface IBar
    {
        void DoBar();
    }

    public class Foo : IFoo
    {
        public void DoFoo()
        {
            Console.WriteLine("Foo");
        }
    }

    public class AlsoFoo : IFoo
    {
        public void DoFoo()
        {
            Console.WriteLine("AlsoFoo");
        }
    }

    public class Bar : IBar
    {
        public void DoBar()
        {
            Console.WriteLine("Bar");
        }
    }

    public class LoggingFooDecorator : IFoo
    {
        private IFoo _impl;

        public LoggingFooDecorator(IFoo impl)
        {
            _impl = impl;
        }

        public void DoFoo()
        {
            Console.WriteLine("LoggingFooDecorator");
            _impl.DoFoo();
        }
    }

    public class Baz
    {
        public Baz(IEnumerable<IFoo> foos, IBar bar)
        {
            foreach (var foo in foos)
            {
                foo.DoFoo();
            }

            bar.DoBar();
        }
    }
}
