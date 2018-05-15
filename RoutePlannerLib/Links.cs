using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{

    ///	<summary>
    ///	Manages	links from a city to another city.
    ///	</summary>
    public class Links
    {
        private List<Link> links = new List<Link>();
        private Cities cities;
        public event EventHandler<RouteRequestEventArgs> RouteRequested;

        public int Count
        {
            get { return links.Count; }
        }

        ///	<summary>
        ///	Initializes	the	Links with	the	cities.
        ///	</summary>
        ///	<param name="cities"></param>
        public Links(Cities cities)
        {
            this.cities = cities;
        }

        ///	<summary>
        ///	Reads a	list of	links from the given file.
        ///	Reads only links where the cities exist.
        ///	</summary>
        ///	<param name="filename">name	of links file</param>
        ///	<returns>number	of read	route</returns>
        public int ReadLinks(string filename)
        {
            var previousCount = Count;

            using (var tr = File.OpenText(filename))
            {
                var tokenLines = tr.GetSplittedLines('\t');
                foreach (var tokens in tokenLines)
                {
                    try
                    {
                        var city1 = cities[tokens[0]];
                        var city2 = cities[tokens[1]];

                        links.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportMode.Rail));
                    }
                    catch (KeyNotFoundException)
                    {
                        //missing cities should be ignored
                    }
                }
            }

            return Count - previousCount;
        }
        public Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
        {
            return Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode, reportProgress));
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode, IProgress<string> reportProgress = null)
        {
            reportProgress?.Report("call to method done");
            RouteRequested?.Invoke(this, new RouteRequestEventArgs(cities[fromCity], cities[toCity], mode));

            //use dijkstra's algorithm to look for all single-source shortest paths
            var visited = new Dictionary<City, DijkstraNode>();
            var pending = new SortedSet<DijkstraNode>(new[]
            {
                new DijkstraNode
                {
                    VisitingCity = cities[fromCity],
                    Distance = 0
                }
            });

            while (pending.Any())
            {
                var cur = pending.Last();
                pending.Remove(cur);

                if (!visited.ContainsKey(cur.VisitingCity))
                {
                    visited[cur.VisitingCity] = cur;

                    foreach (var link in FindAllLinksForCity(cur.VisitingCity, mode))
                        pending.Add(new DijkstraNode
                        {
                            VisitingCity = (link.FromCity.Equals(cur.VisitingCity)) ? link.ToCity : link.FromCity,
                            Distance = cur.Distance + link.Distance,
                            PreviousCity = cur.VisitingCity
                        });
                }
            }
            reportProgress?.Report("dijkstra done");

            //did we find any route?
            if (!visited.ContainsKey(cities[toCity]))
                return null;
            reportProgress?.Report("route search done");
            //create a list of cities that we passed along the way
            var citiesEnRoute = new List<City>();
            for (var c = cities[toCity]; c != null; c = visited[c].PreviousCity)
                citiesEnRoute.Add(c);
            citiesEnRoute.Reverse();
            reportProgress?.Report("created list done");
            //for each city en route, find the link (path) which will be passed along the way. Return all paths as Enumerable.
            IEnumerable<Link> paths = FindLinksToCitiesEnRoute(citiesEnRoute);
            reportProgress?.Report("all paths as enum done");
            return paths.ToList();
        }

        public List<List<Link>> FindAllShortestRoutes()
        {
            var result = new ConcurrentBag<List<Link>>();
            var allCities = cities.GetCities().Select(c => c.Name).ToList();
            var transportModes = Enum.GetValues(typeof(TransportMode)).Cast<TransportMode>().ToList();
            foreach (var source in allCities)
            {
                foreach (var destination in allCities)
                {
                    foreach (var transportMode in transportModes)
                    {
                        result.Add(FindShortestRouteBetween(source, destination, transportMode));
                    }
                }
            }
            return result.ToList();
        }

        private IEnumerable<Link> FindLinksToCitiesEnRoute(List<City> citiesEnRoute)
        {
            for (int i = 1; i < citiesEnRoute.Count; i++)
            {
                var fromCity = citiesEnRoute[i - 1];
                var toCity = citiesEnRoute[i];
                yield return links.First(l => (l.FromCity.Equals(fromCity) && l.ToCity.Equals(toCity)) ||
                                              (l.FromCity.Equals(toCity) && l.ToCity.Equals(fromCity)));
            }

        }

        private IEnumerable<Link> FindAllLinksForCity(City curVisitingCity, TransportMode mode)
        {
            return links.Where(l =>
                (l.FromCity.Equals(curVisitingCity) || l.ToCity.Equals(curVisitingCity)) &&
                l.TransportMode.Equals(mode)).Distinct();
        }

        public City[] FindCities(TransportMode transportMode) =>
            links.Where(l => l.TransportMode.Equals(transportMode))
                .SelectMany(l => new List<City> { l.FromCity, l.ToCity })
                .Distinct()
                .ToArray();

        private class DijkstraNode : IComparable<DijkstraNode>
        {
            public City VisitingCity;
            public double Distance;
            public City PreviousCity;

            public int CompareTo(DijkstraNode other)
            {
                return other.Distance.CompareTo(Distance);
            }

        }

        /// <summary>
        /// Bei wie vielen Links treten die drei bevölkerungsreichsten Städte aller Cities in den Links als Start-Stadt auf?
        /// </summary>
        /// <returns></returns>
        public int GetCountOfThreeBiggestCitiesInLinks()
        {
            var biggestCities = cities.FindBiggest(3).ToList();
            return links.Where(l => biggestCities.Contains(l.FromCity)).Count();
        }


        /// <summary>
        /// Bei wievielen Links treten die Städte mit den drei längsten Städtenamen in den Links insgesamt auf, entweder als Start- oder als Ziel-Stadt?
        /// </summary>
        /// <returns></returns>
        public int GetCountOfThreeCitiesWithLongestNameInLinks()
        {
            var longestCities = cities.FindLongestName(3).ToList();
            return links.Where(l => longestCities.Contains(l.FromCity) || longestCities.Contains(l.ToCity)).Count();
        }

        public List<List<Link>> FindAllShortestRoutesParallel()
        {
            var result = new ConcurrentBag<List<Link>>();
            var allCities = cities.GetCities().AsParallel().Select(c => c.Name).ToList();
            var transportModes = Enum.GetValues(typeof(TransportMode)).Cast<TransportMode>();
            allCities.AsParallel().ForAll(source =>
            {
                foreach (var destination in allCities)
                {
                    foreach (var transportMode in transportModes)
                    {
                        result.Add(FindShortestRouteBetween(source, destination, transportMode));
                    }
                }
            });

            return result.ToList();
        }
    }
}
