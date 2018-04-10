using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private readonly List<City> cities = new List<City>();
        public int ReadCities(string filename)
        {

            using (var textReader = File.OpenText(filename))
            {
                var splittedLines = textReader.GetSplittedLines('\t');
                var converted = splittedLines.Select(sl => new City(sl[0], sl[1], Convert.ToInt32(sl[2]),
                    Convert.ToDouble(sl[3]), Convert.ToDouble(sl[4]))).ToList();
                cities.AddRange(converted);

                return converted.Count();
            }
        }

        public City this[int x]
        {
            get
            {
                if (cities.ElementAtOrDefault(x) == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return cities[x];
            }
            set => cities[x] = value;
        }

        public City this[string searchTerm]
        {
            get
            {
                // lock because of culture
                lock (this)
                {
                    var previousCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    try
                    {
                        if (string.IsNullOrEmpty(searchTerm))
                        {
                            throw new ArgumentNullException();
                        }

                        return cities.Find((c) => string.CompareOrdinal(c.Name.ToUpper(), searchTerm.ToUpper()) == 0) ??
                               throw new KeyNotFoundException();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = previousCulture;
                    }
                }
            }
        }

        public int Count => cities.Count;

        public IList<City> FindNeighbours(WayPoint location, double distance)
        {
            return cities.Where(c => c.Location.Distance(location) <= distance).ToList();
        }

        public IEnumerable<City> FindBiggest(int n)
        {
            return cities.OrderByDescending(c => c.Population).Take(n);
        }

        public IEnumerable<City> FindLongestName(int n)
        {
            return cities.OrderByDescending(c => c.Name.Length).Take(n);
        }

        /// <summary>
        /// Wie gross ist die Bevölkerungszahl der drei Städte mit den kürzesten Städtenamen?
        /// </summary>
        /// <returns></returns>
        public int GetPopulationOfShortestCityNames() =>
                cities.OrderBy(c => c.Name.Length).Take(3).Sum(c => c.Population);


        public IEnumerable<City> GetCities()
        {
            return cities;
        }
    }
}
