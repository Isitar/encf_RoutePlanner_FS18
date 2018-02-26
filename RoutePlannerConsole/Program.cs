using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlannerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to RoutePlanner (Version {Assembly.GetExecutingAssembly().GetName().Version})");
            
            var wayPoint = new Waypoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine($"{wayPoint.Name}: {wayPoint.Latitude}/{wayPoint.Longitude}");
            Console.ReadLine();
        }
    }
}
