using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    public partial class CityTest
    {
        [TestMethod]
        public void TestCityEquality()
        {

            Assert.IsTrue(new City("A", "B", 1, 1.0, 1.0).Equals(new City("A", "B", 1, 1.0, 1.0)));
            // ignore case
            Assert.IsTrue(new City("A", "B", 1, 1.0, 1.0).Equals(new City("a", "b", 1, 1.0, 1.0)));

            // same city, different country, should be false
            Assert.IsFalse(new City("A", "B", 1, 1.0, 1.0).Equals(new City("A", "C", 1, 1.0, 1.0)));
            // different city, same country, should be false
            Assert.IsFalse(new City("A", "B", 1, 1.0, 1.0).Equals(new City("c", "b", 1, 1.0, 1.0)));

            // test also the null case
            Assert.IsFalse(new City("A", "B", 1, 1.0, 1.0).Equals(null));

            // test also the different type case
            Assert.IsFalse(new City("A", "B", 1, 1.0, 1.0).Equals("s"));

        }

    }
}
