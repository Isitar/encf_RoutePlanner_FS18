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
            int counter = 0;
            using (var textReader = File.OpenText(filename))
            {
                var splittedLines = textReader.GetSplittedLines('\t');

                foreach (var splitted in splittedLines)
                {
                    cities.Add(new City(splitted[0], splitted[1], Convert.ToInt32(splitted[2]),
                        Convert.ToDouble(splitted[3]), Convert.ToDouble(splitted[4])));
                    counter++;
                }

                return counter;
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
    }
}
