using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    [DeploymentItem("data/citiesTestDataLab8.txt")]
    [DeploymentItem("data/linksTestDataLab8.txt")]
     public partial class LinksTest
    {
        [TestMethod]
        public void TestCorrectTransportMode()
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab8.txt");

            //check default TransportMode
            var l = new Link(cities[0], cities[1], 0);
            Assert.AreEqual(TransportMode.Car, l.TransportMode, "Links should have a default TransportMode of Car. See Lab03.");


            //check TransportMode of routes
            var links = new Links(cities);
            links.ReadLinks("linksTestDataLab8.txt");

            List<List<Link>> allRoutesSerial = links.FindAllShortestRoutes();
            foreach (var route in allRoutesSerial)
                if (route != null)
                    foreach (var link in route)
                        Assert.AreEqual(TransportMode.Rail, link.TransportMode, "Links from files are rail links. See Lab03.");
        }


        [TestMethod]
        public void TestRouteSerialCorrectness4()
        {
            TestRouteSerialCorrectness("4", 28, 30, 4704, 870, 702, 2559916);
        }

        [TestMethod]
        public void TestRouteSerialCorrectness8()
        {
            TestRouteSerialCorrectness("8", 83, 170, 41334, 7304, 6806, 144185966);
        }

        private void TestRouteSerialCorrectness(string dataFileSuffixNumber, int numberOfCities, int numberOfLinks, 
            int allRoutes, int allNonNullRoutes, int allNonNullNonEmptyRoutes, int expectedHash)
        {
            Cities cities = ReadCities(dataFileSuffixNumber);
            Assert.AreEqual(numberOfCities, cities.Count);

            Links links = ReadLinks(dataFileSuffixNumber, cities);

            Assert.AreEqual(numberOfLinks, links.Count);

            List<List<Link>> allRoutesSerial = links.FindAllShortestRoutes();

            //should return all combinations of routes (cities * routes * TransportModes)
            Assert.AreEqual(allRoutes, allRoutesSerial.Count());

            //filter out non-existing routes
            allRoutesSerial = allRoutesSerial.Where(r => r != null).ToList();

            //should've found a subset of valid routes
            Assert.AreEqual(allNonNullRoutes, allRoutesSerial.Count());

            //filter out zero-length routes
            allRoutesSerial = allRoutesSerial.Where(r => r.Count() > 0).ToList();

            //should've found a subset of non-zero-length routes
            Assert.AreEqual(allNonNullNonEmptyRoutes, allRoutesSerial.Count());

            //sort both lists in a deterministic fashion
            foreach (var list in new[] { allRoutesSerial })
                list.Sort((a, b) => string.Compare(
                        string.Join(":", a.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode)),
                        string.Join(":", b.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode)),
                        StringComparison.InvariantCulture
                    ));

            //serialize lists to a string
            var txtSerial = string.Join("\n", allRoutesSerial.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //the algorithm should deliver the expected result
            var hash = txtSerial.Select((ch, index) => ((int)ch) * index).Aggregate((a, b) => a ^ b);

            Assert.AreEqual(expectedHash, hash);
        }

        private static Links ReadLinks(string dataFileSuffixNumber, Cities cities)
        {
            var links = new Links(cities);
            links.ReadLinks("linksTestDataLab" + dataFileSuffixNumber + ".txt");
            return links;
        }


        [TestMethod]
        public void TestRouteParallelCorrectness4()
        {
            TestRouteParallelCorrectness("4", 28, 30, 4704, 870, 702);
        }

        [TestMethod]
        public void TestRouteParallelCorrectness8()
        {
            TestRouteParallelCorrectness("8", 83, 170, 41334, 7304, 6806);
        }

        private void TestRouteParallelCorrectness(string dataFileSuffixNumber, int numberOfCities, int numberOfLinks, 
            int allRoutes, int allNonNullRoutes, int allNonNullNonEmptyRoutes)
        {
            Cities cities = ReadCities(dataFileSuffixNumber);
            Assert.AreEqual(numberOfCities, cities.Count);

            Links links = ReadLinks(dataFileSuffixNumber, cities);
            Assert.AreEqual(numberOfLinks, links.Count);

            List<List<Link>> allRoutesSerial = links.FindAllShortestRoutes();
            List<List<Link>> allRoutesParallel = links.FindAllShortestRoutesParallel();

            //should return all combinations of routes (cities * routes * TransportModes)
            Assert.AreEqual(allRoutes, allRoutesSerial.Count());
            Assert.AreEqual(allRoutes, allRoutesParallel.Count());

            //filter out non-existing routes
            allRoutesSerial = allRoutesSerial.Where(r => r != null).ToList();
            allRoutesParallel = allRoutesParallel.Where(r => r != null).ToList();

            //should've found a subset of valid routes
            Assert.AreEqual(allNonNullRoutes, allRoutesSerial.Count());
            Assert.AreEqual(allNonNullRoutes, allRoutesParallel.Count());

            //filter out zero-length routes
            allRoutesSerial = allRoutesSerial.Where(r => r.Count() > 0).ToList();
            allRoutesParallel = allRoutesParallel.Where(r => r.Count() > 0).ToList();

            //should've found a subset of non-zero-length routes
            Assert.AreEqual(allNonNullNonEmptyRoutes, allRoutesSerial.Count());
            Assert.AreEqual(allNonNullNonEmptyRoutes, allRoutesParallel.Count());

            //sort both lists in a deterministic fashion
            foreach (var list in new[] { allRoutesSerial, allRoutesParallel })
                list.Sort((a, b) => string.Compare(
                        string.Join(":", a.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode)),
                        string.Join(":", b.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode)),
                        StringComparison.InvariantCulture
                    ));

            //serialize lists to a string
            var txtSerial = string.Join("/", allRoutesSerial.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));
            var txtParallel = string.Join("/", allRoutesParallel.Select(i => string.Join(":", i.Select(l => l.FromCity.Name + " " + l.ToCity.Name + " " + l.TransportMode))));

            //both algorithms should deliver the same result
            Assert.AreEqual(txtSerial, txtParallel);
        }

        private static Cities ReadCities(string dataFileSuffixNumber)
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab" + dataFileSuffixNumber + ".txt");
            return cities;
        }

        [TestMethod]
        public void TestRouteParallelSpeed()
        {
            var cities = new Cities();
            cities.ReadCities("citiesTestDataLab8.txt");
            var routes = new Links(cities);
            routes.ReadLinks("linksTestDataLab8.txt");

            //warmup
            routes.FindAllShortestRoutesParallel();

            //execute and measure time
            var timeA = DateTime.Now;
            routes.FindAllShortestRoutes();
            var timeB = DateTime.Now;
            routes.FindAllShortestRoutesParallel();
            var timeC = DateTime.Now;
            var oldAffinity = Process.GetCurrentProcess().ProcessorAffinity;
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)1;
            routes.FindAllShortestRoutesParallel();
            Process.GetCurrentProcess().ProcessorAffinity = oldAffinity;
            var timeD = DateTime.Now;

            var factor = (timeC - timeB).TotalSeconds / (timeB - timeA).TotalSeconds;
            var slowdown = (timeD - timeC).TotalSeconds / (timeB - timeA).TotalSeconds;

            Trace.WriteLine($"Factor: {factor:F2}, Slowdown: {slowdown:F2}");
            Trace.WriteLine($"Processors: {Environment.ProcessorCount}");

            //parallel execution on a single core shouldn't be much slower then the serial execution
            Assert.IsTrue(slowdown < 2);

            switch (Environment.ProcessorCount)
            {
                case 1:
                    break;
                case 2:
                    //expect at least 25% reduction with 2 cores
                    Assert.IsTrue(factor < 0.75);
                    break;
                case 3:
                    //expect at least 35% reduction with 3 cores
                    Assert.IsTrue(factor < 0.65);
                    break;
                default:
                    //expect at least 45% reduction with 4+ cores
                    Assert.IsTrue(factor < 0.60);
                    break;
            }
        }
    }
}
