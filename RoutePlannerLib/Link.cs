using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public enum TransportMode
    {
        Ship,
        Rail,
        Flight,
        Car,
        Bus,
        Tram
    };

    /// <summary>
    /// Represents a link between two cities with its distance
    /// </summary>
    public class Link : IComparable
    {
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public double Distance { get; set; }
        public TransportMode TransportMode { get; set; } = TransportMode.Car;

        public Link(City fromCity, City toCity, double distance)
        {
            FromCity = fromCity;
            ToCity = toCity;
            Distance = distance;
        }

        public Link(City fromCity, City toCity, double distance, TransportMode transportMode) : this(fromCity, toCity, distance)
        {
            TransportMode = transportMode;
        }

        /// <inheritdoc />
        public int CompareTo(object o)
        {
            return Distance.CompareTo(((Link)o).Distance);
        }

        /// <summary>
        /// checks if both cities of the link are included in the passed city list
        /// </summary>
        /// <param name="cities">list of city objects</param>
        /// <returns>true if both link-cities are in the list</returns>
        internal bool IsIncludedIn(List<City> cities)
        {
  
            var foundFrom = false;
            var foundTo = false;
            foreach (var c in cities)
            {
                if (!foundFrom && c.Name == FromCity.Name)
                    foundFrom = true;

                if (!foundTo && c.Name == ToCity.Name)
                    foundTo = true;
            }

            return foundTo && foundFrom;
        }
    }
}
