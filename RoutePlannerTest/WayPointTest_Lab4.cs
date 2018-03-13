using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    
     public partial class WayPointTest
    {

        [TestMethod]
        public void TestWayPointOperators()
        {
            var wp1 = new WayPoint("Home", 10.4, 20.8);
            var wp2 = new WayPoint("Target", 1.2, 2.4);

            var addWp = wp1 + wp2;
            if (object.ReferenceEquals(addWp, wp1) || object.ReferenceEquals(addWp, wp2))
                Assert.Fail("Operations must put results in *new* objects");

            Assert.AreEqual(wp1.Name, addWp.Name);
            Assert.AreEqual(wp1.Latitude + wp2.Latitude, addWp.Latitude);
            Assert.AreEqual(wp1.Longitude + wp2.Longitude, addWp.Longitude);

            var minWp = wp1 - wp2;
            if (object.ReferenceEquals(minWp, wp1) || object.ReferenceEquals(minWp, wp2))
                Assert.Fail("Operations must put results in *new* objects");

            Assert.AreEqual(minWp.Name, wp1.Name);
            Assert.AreEqual(minWp.Latitude, wp1.Latitude - wp2.Latitude);
            Assert.AreEqual(minWp.Longitude, wp1.Longitude - wp2.Longitude);

            var minWp2 = wp2 - wp1;
            Assert.AreEqual(minWp2.Name, wp2.Name);
            Assert.AreEqual(minWp2.Latitude, wp2.Latitude - wp1.Latitude);
            Assert.AreEqual(minWp2.Longitude, wp2.Longitude - wp1.Longitude);
        }

    }
}
