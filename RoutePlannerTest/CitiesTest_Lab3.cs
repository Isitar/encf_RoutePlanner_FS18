using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Reflection;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    public partial class CitiesTest
    {
        private const string CitiesTestFile = "citiesTestDataLab3.txt";
        private const string LinksTestFile = "linksTestDataLab3.txt";
        private Cities cities = new Cities();

        [TestInitialize]
        public void Setup()
        {
            cities = new Cities();
            Assert.AreEqual(11, cities.ReadCities(CitiesTestFile));
        }

        [TestMethod]
        public void TestFindCityWithNullIndexer()
        {
            try
            {
                var x = cities[null];
                Assert.Fail("Indexer Cities[string] should throw a ArgumentNullException when null passed.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void TestTask1FindCityInCities()
        {
            try
            {
                var x = cities["noCity"];
                Assert.Fail("Indexer Cities[string] should throw a KeyNotFoundException when the supplied City cannot be found.");
            }
            catch (KeyNotFoundException)
            {
            }

            Assert.AreEqual("Zürich", cities["Zürich"].Name);

            // should be case insensitive
            Assert.AreEqual("Zürich", cities["züRicH"].Name);

            // should be case insensitive, even in "weird" cultures,
            // see http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Assert.AreEqual("Zürich", cities["züRicH"].Name);
            Assert.AreEqual("Zürich", cities["züRIcH"].Name);
            Thread.CurrentThread.CurrentCulture = previousCulture;

            // should be picky about spaces
            try
            {
                var x = cities["züRicH "];
                Assert.Fail("Indexer Cities[string] should be picky about leading/trailing spaces.");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void TestFindCityInCitiesIndexerCaseInsenstive()
        {
            // should be case insensitive, even in "weird" cultures,
            // see http://www.moserware.com/2008/02/does-your-code-pass-turkey-test.html
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Assert.AreEqual("Zürich", cities["züRicH"].Name);
            Assert.AreEqual("Zürich", cities["züRIcH"].Name);
            Thread.CurrentThread.CurrentCulture = previousCulture;

            // should be picky about spaces
            try
            {
                var x = cities["züRicH "];
                Assert.Fail("Indexer Cities[string] should be picky about leading/trailing spaces.");
            }
            catch (KeyNotFoundException)
            {
            }
        }


        [TestMethod]
        public void TestTask2CitiesContentCheck()
        {
            CheckCity(cities["Zürich"], "Switzerland", 348059, 47.38, 8.54);
            CheckCity(cities["winterThur"], "Switzerland", 91588, 47.51, 8.72);
            CheckCity(cities["Lyon"], "France", 443859, 45.76, 4.83);
            CheckCity(cities["Frauenfeld"], "Switzerland", 22044, 47.56, 8.89);
            CheckCity(cities["Lausanne"], "Switzerland", 117371, 46.52, 6.62);
        }

        private static void CheckCity(City city, string countryToCheck, int populationToCheck, double latToCheck, double lngToCheck)
        {
            Assert.AreEqual(countryToCheck, city.Country);
            Assert.AreEqual(populationToCheck, city.Population);
            Assert.AreEqual(latToCheck, city.Location.Latitude);
            Assert.AreEqual(lngToCheck, city.Location.Longitude);
        }

        [TestMethod]
        public void TestCitiesCountExistsAndIsReadonly()
        {
            var propertyInfo = typeof(Cities).GetProperty("Count");
            if (propertyInfo != null)
            {
                Assert.IsTrue(propertyInfo.CanRead);
                Assert.IsFalse(propertyInfo.CanWrite);
            }
            else
            {
                Assert.Fail("Property Count not existing.");
            }
        }
 
 
        [TestMethod]
        public void TestCitiesHasNoVariableWithNameCount()
        {
            var fields = typeof(Cities).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.Name.ToLowerInvariant().Contains("count"))
                {
                    Assert.Fail("No backing field for Count ist allowed. Use citiesList Count and pass through.");
                }
            }
        }
               
        // a very simple test to verify console output
        // just checks one line
        private void VerifyConsoleOut(Stream ms, string requestedString)
        {
            ms.Position = 0;
            using (var sw = new StreamReader(ms))
            {
                // just check if the following line is in the file
                string line;
                while ((line = sw.ReadLine()) != null)
                {
                    if (line.Contains(requestedString))
                    {
                        Assert.IsTrue(true);
                        return;
                    }
                }
                Assert.Fail();
            }
        }
    }
}
