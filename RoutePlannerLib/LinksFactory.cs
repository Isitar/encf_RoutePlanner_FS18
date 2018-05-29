using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class LinksFactory
    {
        public static ILinks Create(Cities cities)
        {
            return Create(cities, Properties.Settings.Default.RouteAlgorithm);
        }

        public static ILinks Create(Cities cities, string algorithmClassName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies.Where(a => a.GetTypes().Select(t=>t.FullName).Any(t => t.Equals(algorithmClassName))))
            {
                try
                {
                    return (ILinks) Activator.CreateInstance(assembly.GetType(algorithmClassName), cities);
                }
                catch
                {
                    // ignored
                }
            }

            throw new NotSupportedException();
        }

    }
}
