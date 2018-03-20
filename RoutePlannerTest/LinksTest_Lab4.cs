using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public partial class LinksTest
    {
        Cities cities;
        Links links;

        private const string CitiesTestFile2 = "citiesTestDataLab4.txt";
        private const string LinksTestFile2 = "linksTestDataLab4.txt";


        [TestInitialize]
        public void Setup()
        {
            cities = new Cities();
            cities.ReadCities(CitiesTestFile2);

            links = new Links(cities);
            links.ReadLinks(LinksTestFile2);
        }

        [TestMethod]
        public void TestCorrectNumberofReadLinks()
        {
            // make a second read call
            var count = links.ReadLinks(LinksTestFile2);
            // should have read 30 
            Assert.AreEqual(30, count);

            // total number should be 60 now 
            Assert.AreEqual(60, links.Count);
        }

        [TestMethod]
        public void TestFindLinksToCitiesEnRouteDoesNotCreateNewLinks()
        {
            // find the same route twice
            var route1 = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);
            var route2 = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);

            // same link amount has to be returned
            Assert.AreEqual(route1.Count, route2.Count);

            // same links have to be returned, its not allowed to create own links while converting
            for (var i = 0; i < route1.Count; i++)
            {
                Assert.IsTrue(ReferenceEquals(route1[i], route2[i]), "Same link object-instances have to in same order in route1 and route2");
            }
        }

        [TestMethod]
        public void TestTransportationMode()
        {
            // find the same route twice
            var route1 = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);
            Assert.IsTrue(route1.All(l => l.TransportMode == TransportMode.Rail), "Transportation mode shall be train, as requested in method call.");
        }


        [TestMethod]
        public void TestFindRoutes()
        {
            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Assert.AreEqual(28, cities.Count);

            // test available cities
            var route = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);
            Assert.IsNotNull(route);
            Assert.AreEqual(route.Count, expectedLinks.Count);

            for (var i = 0; i < route.Count; i++)
            {
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == route[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == route[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == route[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == route[i].FromCity.Name));
            }

            // test some other route
            route = links.FindShortestRouteBetween("Zürich", "Milano", TransportMode.Rail);
            Assert.AreEqual(5, route.Count);

            // test when no routes can be found
            route = links.FindShortestRouteBetween("Zürich", "Le Havre", TransportMode.Rail);
            Assert.IsNull(route);

            try
            {
                route = links.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw a KeyNotFoundException");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void TestIfSplittedLineMethodIsCalled()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Links.cs", "ReadLinks", "GetSplittedLines");
        }
    }
}
