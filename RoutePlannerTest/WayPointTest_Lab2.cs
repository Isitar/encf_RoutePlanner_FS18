using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// This test class contains all tests for class WayPoint for Lab 2
    /// 
    /// REMARK:
    /// We split the tests per class and lab into separate files using the partial class concepts.
    /// This allows us to distribute additional test for each class with each new lab.
    ///</summary>
    [TestClass]
    public partial class WayPointTest
    {
        private const string citiesTestFile = "citiesTestDataLab2.txt";

        /// <summary>
        /// A test for WayPoint Constructor        
        /// </summary>
        [TestMethod]
        public void TestWayPointValidConstructor()
        {
            var target = new WayPoint("Windisch", 0.564, 0.646);

            Assert.AreEqual("Windisch", target.Name);
            Assert.AreEqual(0.564, target.Latitude);
            Assert.AreEqual(0.646, target.Longitude);
        }

        [TestMethod]
        public void TestWayPointToString()
        {
            // test complete way point
            var target = new WayPoint("Windisch", 0.564, 0.646);
            Assert.AreEqual("WayPoint: Windisch 0.56/0.65", target.ToString());

            // test no-name case
            target = new WayPoint(null, 0.564, 0.646);
            Assert.AreEqual("WayPoint: 0.56/0.65", target.ToString());

            // test for correct formatting with 2 decimal places
            var targetRound = new WayPoint("Testtest", 1.0, 0.50);
            Assert.AreEqual("WayPoint: Testtest 1.00/0.50", targetRound.ToString());
        }
        

        [TestMethod]
        public void TestCultureHandling()
        {
            var target = new WayPoint("Windisch", 0.564, 0.646);

            // test whether formatting works correctly in all cultures
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            var newCulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Assert.AreEqual("WayPoint: Windisch 0.56/0.65", target.ToString());
            Assert.AreEqual(Thread.CurrentThread.CurrentCulture, newCulture);

            newCulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("de-CH");
            Assert.AreEqual("WayPoint: Windisch 0.56/0.65", target.ToString());
            Assert.AreEqual(Thread.CurrentThread.CurrentCulture, newCulture);

            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [TestMethod]
        public void TestWayPointDistanceCalculation()
        {
            var bern = new WayPoint("Bern", 46.95, 7.44);
            var tripolis = new WayPoint("Tripolis", 32.876174, 13.187507);
            var actual = bern.Distance(tripolis);
            Assert.IsFalse(double.IsNaN(actual));
            Assert.AreEqual(1638.74410788167, actual, 0.001);

            actual = tripolis.Distance(bern);
            Assert.IsFalse(double.IsNaN(actual));
            Assert.AreEqual(1638.74410788167, actual, 0.001);
        }


        /// <summary>
        /// Method names in C# have to start with capital letters
        /// </summary>
        [TestMethod]
        public void TestWaypointMethodNames()
        {
            TestHelpers.CheckMethodNamesAreCapitalized(typeof(WayPoint));
        }

    }
}
