using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private List<City> cities = new List<City>();
        public int ReadCities(string filename)
        {
            int counter = 0;
            var lines = File.ReadAllLines(filename);

            foreach (var line in lines)
            {
                var splitted = line.Split('\t');
                cities.Add(new City(splitted[0], splitted[1], Convert.ToInt32(splitted[2]), Convert.ToDouble(splitted[3]), Convert.ToDouble(splitted[4])));
                counter++;
            }
            return counter;
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
                var previousCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                try
                {
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        throw new ArgumentNullException();
                    }

                    var foundCity = cities.Find((c) => string.CompareOrdinal(c.Name.ToUpper(), searchTerm.ToUpper()) == 0);
                    if (foundCity == null)
                    {
                        throw new KeyNotFoundException();
                    }

                    return foundCity;
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = previousCulture;
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
