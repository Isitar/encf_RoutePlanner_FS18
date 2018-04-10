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

        public virtual DateTime GetCurrentDate { get { return DateTime.Now; } }
        private List<Tuple<City, DateTime>> cityRequestsDate = new List<Tuple<City, DateTime>>();

        /// <summary>
        ///  Was waren die drei bevölkerungsreichsten Städte, die an einem bestimmten Tag abgefragt wurden?
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public IEnumerable<City> GetBiggestCityOnDay(DateTime day)
        {
            return null;
        }

        /// <summary>
        /// Geben Sie die drei Städte mit den längsten Städtenamen zurück, die im gegebenen Zeitraum(inklusive from und to) abgefragt wurden.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<City> GetLongestCityNamesWithinPeriod(DateTime from, DateTime to)
        {
            return null;
        }

        /// <summary>
        /// Welche Städte wurden in den letzten zwei Wochen nie als Ziel angefragt?
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        public IEnumerable<City> GetNotRequestedCities(Cities cities)
        {
            return null;
        }
    }
}
