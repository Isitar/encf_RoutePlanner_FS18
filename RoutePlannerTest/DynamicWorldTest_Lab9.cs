using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    public class DynamicWorldTest
    {

        [TestMethod]
        public void TestDynamicWorld()
        {
            var cities = new Cities();

            cities.ReadCities("citiesTestDataLab2.txt");

            dynamic world = new World(cities);

            dynamic karachi = world.Karachi();
            Assert.AreEqual("Karachi", karachi.Name);
            Assert.AreEqual(cities["Karachi"], karachi);

            string notFound = world.Entenhausen();
            Assert.AreEqual("The city \"Entenhausen\" does not exist!", notFound);
        }
    }
}
