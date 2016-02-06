using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInjector.Conventions.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();

            container.RegisterByConvention(cfg =>
            {
                cfg.AssemblyFilter = x => x.FullName.StartsWith("SimpleInjector.Conventions.ConsoleApplication");
            });

            container.Verify();

            Console.WriteLine("What I Have....");
            foreach (var item in container.GetCurrentRegistrations())
            {
                Console.WriteLine(item.VisualizeObjectGraph());
            }

            Console.WriteLine();
            Console.WriteLine("Loading Baz...");
            var baz = container.GetInstance<Baz>();

            Console.ReadKey();
        }
    }
}
