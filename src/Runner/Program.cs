using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            
            for (int i = 0; i < ass.Length; i++)
            {
                System.Console.WriteLine(ass[i].FullName);
            }
            var ass2 = GetReferencingAssemblies("Samples").Where(x=>x.FullName.StartsWith("Samples")).FirstOrDefault();
            if(ass2 != null)
            {
                var dictTypes = new Dictionary<int,Type>();
                for(var i = 0; i < ass2.ExportedTypes.Count(); i++)
                {
                    dictTypes.Add(i, ass2.ExportedTypes.ElementAt(i));
                    Console.WriteLine($"{i},{ass2.ExportedTypes.ElementAt(i)}");
                }
                var curInsance = Activator.CreateInstance(dictTypes[4]);
                curInsance.GetType().GetMethod("SocketServerTest", BindingFlags.Public | BindingFlags.Static).Invoke(null,new object[] { new string[] { "5000" } });
            }
        }
        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name == (assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }
    }
}
