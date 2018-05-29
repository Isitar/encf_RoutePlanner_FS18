using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.IO;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public class LinksFactoryTest
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";

        private const string LinksTestFile = "linksTestDataLab4.txt";

		private const string AppConfigFile = "/RoutePlannerTest/bin/Debug/RoutePlannerLib.dll.config";

		
        [TestMethod]
        public void TestLoadDynamicValid()
        {
            var cities = new Cities();
            // just test for correct dynamic creation of valid routed class from config
            ILinks links = LinksFactory.Create(cities);
            Assert.IsInstanceOfType(links, typeof(ILinks));
            
            // now test for correct dynamic creation of valid routed class passed as string
            links = LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Links");
            Assert.IsInstanceOfType(links, typeof(ILinks));
        }
        
        [TestMethod]
        public void TestLoadDynamicInvalid()
        {
            var cities = new Cities();
            // pass a name of a class that does not exist
            try
            {
                ILinks links = LinksFactory.Create(cities, "Class.Does.Not.Exist");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch(NotSupportedException)
            {
            }

            // pass a name of a class that exists, but does not implement the interface
            try
            {
                ILinks links = LinksFactory.Create(cities, "Cities");
                Assert.Fail("Should throw a NotSupportedException");
            }
            catch (NotSupportedException)
            {
            }
        }


        [TestMethod]
        public void TestLoadAndRunDynamicSecondImpl()
        {
            var defaultSettings = Assembly.GetAssembly(typeof(ILinks))
                .GetType("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Properties.Settings")
                .GetProperty("Default")
                .GetValue(null);
            defaultSettings.GetType().GetMethod("Reset").Invoke(defaultSettings, new object[] { });
            //defaultSettings.GetType().GetMethod("Reload").Invoke(defaultSettings, new object[] { });

            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            ILinks links = LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.TestLinksValidConstructor");
            Assert.AreEqual(42, links.ReadLinks("nonsense"));

            ILinks links2 = LinksFactory.Create(cities);
            Assert.AreNotEqual(links.GetType(), links2.GetType());

            try
            {
                try
                {
                    defaultSettings.GetType().GetProperty("RouteAlgorithm").SetValue(defaultSettings, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.TestLinksValidConstructor");
                }
                catch
                {
                    Assert.Fail("Make sure there is a String-property called \"RouteAlgorithm\" with er-scope.");
                }

                ILinks links3 = LinksFactory.Create(cities);
                Assert.AreEqual(links.GetType(), links3.GetType());
            }
            finally
            {
                defaultSettings.GetType().GetMethod("Reset").Invoke(defaultSettings, new object[] { });
            }

            try
            {
                LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab5Test.TestLinksInvalidConstructor");
                Assert.Fail("Should throw a NotSupportedException, because it doesn't have the right constructor");
            }
            catch(NotSupportedException)
            {
            }

            try
            {
                LinksFactory.Create(cities, "Fhnw.Ecnf.RoutePlanner.RoutePlannerTest.Lab10Test.TestLinksNoInterface");
                Assert.Fail("Should throw a NotSupportedException, because ILinks is not implemented");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestLoadAndRunDynamicClassicImpl()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            ILinks links = LinksFactory.Create(cities);

            links.ReadLinks(LinksTestFile);

            Assert.AreEqual(28, cities.Count);

            // test available cities
            List<Link> resultLinks = links.FindShortestRouteBetween("Zürich", "Basel", TransportMode.Rail);

            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                                       new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Assert.IsNotNull(resultLinks);
            Assert.AreEqual(expectedLinks.Count, resultLinks.Count);

            for (int i = 0; i < resultLinks.Count; i++)
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == resultLinks[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == resultLinks[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == resultLinks[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == resultLinks[i].FromCity.Name));

            try
            {
                resultLinks = links.FindShortestRouteBetween("doesNotExist", "either", TransportMode.Rail);
                Assert.Fail("Should throw a KeyNotFoundException");
            }
            catch(KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void TestAppConfigExists()
        {
            string fileContent;

            try
            {
                var projectRootDir = GetProjectRootDir();
                var appConfigPath = projectRootDir + AppConfigFile;
                Console.WriteLine($"Current dir: {appConfigPath}");

                using (TextReader reader = new StreamReader(appConfigPath))
                {
                    // read the whole file into a string
                    fileContent = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Assert.Fail("Console App config file not found. Make sure you added the app.config via project settings.");
                return;
            }

            // now check for algorithm entries
            Assert.IsTrue(fileContent.Contains("RouteAlgorithm"));        }

        private  string GetProjectRootDir()
        {
            // go dir up til you find the root planner console
            bool found = false;
            string parentDir = Directory.GetCurrentDirectory();
            while (!found)
            {
                parentDir = Directory.GetParent(parentDir).FullName;
                Console.WriteLine($"Proj dir: {parentDir}");
                if (Array.Find(Directory.GetDirectories(parentDir), s => s.EndsWith("RoutePlannerTest")) != null)
                {
                    found = true;
                }
            }
            return parentDir;
        }
    }		    

    class TestLinksInvalidConstructor : ILinks
    {
        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }

    public class TestLinksValidConstructor : ILinks
    {
        public TestLinksValidConstructor(Cities c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }

    class TestLinksNoInterface
    {
        public TestLinksNoInterface(Cities c)
        {
        }

        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportMode mode)
        {
            return null;
        }

        public int ReadLinks(string filename)
        {
            return 42;
        }
    }	
}