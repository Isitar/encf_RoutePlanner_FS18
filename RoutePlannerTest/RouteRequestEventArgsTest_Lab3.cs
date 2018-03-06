using System;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    public class RouteRequestEventArgsTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var fromCity = new City("Bern", "Schweiz", 75000, 47.479319847061966, 8.212966918945312);
            var toCity = new City("Schönefeld", "Schweiz", 75000, 47.479319847061966, 8.212966918945312);

            var eventArgs = new RouteRequestEventArgs(fromCity, toCity, TransportMode.Car);

            Assert.AreEqual(fromCity, eventArgs.FromCity);
            Assert.AreEqual(toCity, eventArgs.ToCity);
            Assert.AreEqual(TransportMode.Car, eventArgs.Mode);

        }
    }
}
