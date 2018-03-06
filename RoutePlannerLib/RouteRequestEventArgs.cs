using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : EventArgs
    {
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public TransportMode Mode { get; set; }

        public RouteRequestEventArgs(City fromCity, City toCity, TransportMode mode)
        {
            FromCity = fromCity;
            ToCity = toCity;
            Mode = mode;
        }
    }
}
