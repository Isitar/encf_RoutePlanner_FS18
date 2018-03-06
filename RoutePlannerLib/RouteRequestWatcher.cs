using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        private readonly Dictionary<City, int> cityCounter = new Dictionary<City, int>();

        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            lock (this)
            {
                if (!cityCounter.ContainsKey(e.ToCity))
                {
                    cityCounter.Add(e.ToCity, 1);
                }
                else
                {
                    cityCounter[e.ToCity]++;
                }
            }
            Console.WriteLine("Current Request State");
            Console.WriteLine(new String('-',20));
            foreach (var kvp in cityCounter)
            {
                Console.WriteLine($"ToCity: {kvp.Key.Name} has been requested {kvp.Value} times");
            }
        }

        public int GetCityRequests(City city)
        {
            return !cityCounter.ContainsKey(city) ? 0 : cityCounter[city];
        }
    }
}
