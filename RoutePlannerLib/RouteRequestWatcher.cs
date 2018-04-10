using System;
using System.Collections.Generic;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        private readonly Dictionary<City, int> cityCounter = new Dictionary<City, int>();
        public virtual DateTime GetCurrentDate { get { return DateTime.Now; } }
        private List<Tuple<City, DateTime>> cityRequestsDate = new List<Tuple<City, DateTime>>();

        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {

            if (!cityCounter.ContainsKey(e.ToCity))
            {
                cityCounter.Add(e.ToCity, 1);
            }
            else
            {
                cityCounter[e.ToCity]++;
            }

            cityRequestsDate.Add(new Tuple<City, DateTime>(e.ToCity, GetCurrentDate));

            Console.WriteLine("Current Request State");
            Console.WriteLine(new String('-', 20));
            foreach (var kvp in cityCounter)
            {
                Console.WriteLine($"ToCity: {kvp.Key.Name} has been requested {kvp.Value} times");
            }
        }

        public int GetCityRequests(City city)
        {
            return !cityCounter.ContainsKey(city) ? 0 : cityCounter[city];
        }


        /// <summary>
        ///  Was waren die drei bevölkerungsreichsten Städte, die an einem bestimmten Tag abgefragt wurden?
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public IEnumerable<City> GetBiggestCityOnDay(DateTime day)
        {
            var citiesRequestedOnDay = cityRequestsDate.Where(c => c.Item2.Equals(day)).Select(c => c.Item1).Distinct();
            return citiesRequestedOnDay.OrderByDescending(c=>c.Population).Take(3);
        }

        /// <summary>
        /// Geben Sie die drei Städte mit den längsten Städtenamen zurück, die im gegebenen Zeitraum(inklusive from und to) abgefragt wurden.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<City> GetLongestCityNamesWithinPeriod(DateTime from, DateTime to)
        {
            var citiesRequestedInPeriod = cityRequestsDate.Where(c => c.Item2 >= from && c.Item2 <= to).Select(c => c.Item1).Distinct();

            return citiesRequestedInPeriod.OrderByDescending(c => c.Name.Length).Take(3);
        }

        /// <summary>
        /// Welche Städte wurden in den letzten zwei Wochen nie als Ziel angefragt?
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        public IEnumerable<City> GetNotRequestedCities(Cities cities)
        {
            var citiesRequestedInLastTwoWeek = cityRequestsDate.Where(c => c.Item2 >= GetCurrentDate.AddDays(-14) && c.Item2 <= GetCurrentDate).Select(c => c.Item1);
            return cities.GetCities().Where(c => !citiesRequestedInLastTwoWeek.Contains(c));
        }
    }
}
