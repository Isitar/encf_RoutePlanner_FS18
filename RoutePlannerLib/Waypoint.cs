using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public WayPoint(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return FormattableString.Invariant($"WayPoint: {(string.IsNullOrEmpty(Name) ? "" : Name + " ")}{Latitude:F2}/{Longitude:F2}");
        }

        /// <summary>
        /// Calculates the distance between this waypoint and its target
        /// </summary>
        /// <param name="target"></param>
        /// <returns>Distance in km</returns>
        public double Distance(WayPoint target)
        {

            double r = 6371;
            double latA = DegreeToRadian(Latitude);
            double latB = DegreeToRadian(target.Latitude);
            double longA = DegreeToRadian(Longitude);
            double longB = DegreeToRadian(target.Longitude);

            return r * Math.Acos(Math.Sin(latA) * Math.Sin(latB) +
                      Math.Cos(latA) * Math.Cos(latB) * Math.Cos(longA - longB));
        }

        private double DegreeToRadian(double deg)
        {
            return (Math.PI / 180) * deg;
        }
    }
}
