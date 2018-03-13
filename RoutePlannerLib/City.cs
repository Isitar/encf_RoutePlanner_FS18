using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City : IEquatable<City>
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public WayPoint Location { get; set; }

        public City(string name, string country, int population, double latitude, double longitude)
        {
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(Name, latitude, longitude);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as City);
        }

        public bool Equals(City other)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var retVal = other != null &&
                             Name.ToUpper().Equals(other.Name.ToUpper()) &&
                             Country.ToUpper().Equals(other.Country.ToUpper());

            Thread.CurrentThread.CurrentCulture = oldCulture;
            return retVal;
        }

        public static bool operator ==(City city1, City city2)
        {
            return EqualityComparer<City>.Default.Equals(city1, city2);
        }

        public static bool operator !=(City city1, City city2)
        {
            return !(city1 == city2);
        }
    }
}
