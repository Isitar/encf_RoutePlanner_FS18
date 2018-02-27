using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// This test class contains all tests for class Cities for Lab 2
    /// 
    /// REMARK:
    /// We split the tests per class and lab into separate files using the partial class concepts.
    /// This allows us to distribute additional test for each class with each new lab.
    ///</summary>
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    public partial class CitiesTest
    {
        private const string citiesTestFile = "citiesTestDataLab2.txt";

        [TestMethod]
        public void TestFileClose()
        {
            //create a new empty file
            var tempFN=Path.GetTempFileName();
            using (var fs = new StreamWriter(tempFN))
            {
            }

            Assert.AreEqual(0, new Cities().ReadCities(tempFN));

            //see whether someone is still using the file --> shouldn't throw an exception
            File.Delete(tempFN);
        }

        [TestMethod]
        public void TestReadCitiesFileMissing()
        {
            var cities = new Cities();

            try
            {
                cities.ReadCities("doesnotexist");
                Assert.Fail();
            }
            catch(FileNotFoundException)
            {
            }
            catch
            {
                Assert.Fail("Expected is that file cannot be found. Please throw the default FileNotFoundException in that case.");
            }
        }

        [TestMethod]
        public void TestReadCitiesAndIndexer()
        {
            const int expectedCities = 10;
            var cities = new Cities();

            Assert.AreEqual(expectedCities, cities.ReadCities(citiesTestFile));

            Assert.AreEqual(expectedCities, cities.Count);

            // read cities once again; cities should be added to the list
            Assert.AreEqual(expectedCities, cities.ReadCities(citiesTestFile));

            // total count should be doubled
            Assert.AreEqual(2 * expectedCities, cities.Count);

            //verify first and last city
            Assert.AreEqual("Mumbai", cities[0].Name);
            Assert.AreEqual("Jakarta", cities[9].Name);

            // check for invalid index
            try
            {
                var c = cities[-1];
                Assert.Fail("Invalid index not handled properly");
            }
            catch(ArgumentOutOfRangeException _iore)
            {
                Assert.IsTrue(_iore.Message.Length>2, "IndexOutOfRangeException has no meaningful description");
            }
            catch
            {
                Assert.Fail("Wrong exception type thrown on invalid index");
            }

            try
            {
                var c = cities[100];
                Assert.Fail("Invalid index not handled properly");
            }
            catch (ArgumentOutOfRangeException _iore)
            {
                Assert.IsTrue(_iore.Message.Length > 2, "IndexOutOfRangeException has no meaningful description");
            }
            catch
            {
                Assert.Fail("Wrong exception type thrown on invalid index");
            }

            // test in other cultures
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Assert.AreEqual(expectedCities, new Cities().ReadCities(citiesTestFile));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-CH");
            Assert.AreEqual(expectedCities, new Cities().ReadCities(citiesTestFile));
            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [TestMethod]
        public void TestFindNeighbours()
        {
            var cities = new Cities();
            cities.ReadCities(citiesTestFile);

            var loc = cities[0].Location;

            IEnumerable<City> neighbors = cities.FindNeighbours(loc, 2000);

            //verifies if the correct cities were found
            Assert.AreEqual(4, neighbors.Count());
            Assert.IsTrue(neighbors.Any(c => c.Name == "Mumbai"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Karachi"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Dhaka"));
            Assert.IsTrue(neighbors.Any(c => c.Name == "Dilli"));
        }

        
        /// <summary>
        /// Method names in C# have to start with capital letters
        /// </summary>
        [TestMethod]
        public void TestCitiesMethodNames()
        {
            TestHelpers.CheckMethodNamesAreCapitalized(typeof(Cities));
        }
    }
}
