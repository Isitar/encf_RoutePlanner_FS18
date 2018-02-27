using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// This test class contains all tests for class City for Lab 2
    /// 
    /// REMARK:
    /// We split the tests per class and lab into separate files using the partial class concepts.
    /// This allows us to distribute additional test for each class with each new lab.
    ///</summary>
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    public partial class CityTest
    {
        /// <summary>
        ///A test for City Constructor        
        /// </summary>
        [TestMethod]
        public void TestCityValidConstructor()
        {
            const double latitude = 47.479319847061966;
            const double longitude = 8.212966918945312;
            const int population = 75000;
            const string name = "Bern";
            const string country = "Schweiz";

            var bern = new City(name, country, population, latitude, longitude);

            Assert.AreEqual(name, bern.Name);
            Assert.AreEqual(name, bern.Location.Name); // city name == wayPoint name
            Assert.AreEqual(population, bern.Population);
            Assert.AreEqual(longitude, bern.Location.Longitude, 0.001);
            Assert.AreEqual(latitude, bern.Location.Latitude, 0.001);
        }

  
      /// <summary>
        /// Method names in C# have to start with capital letters
        /// </summary>
        [TestMethod]
        public void TestCityMethodNames()
        {
            TestHelpers.CheckMethodNamesAreCapitalized(typeof(City));
        }

  
    }
}
