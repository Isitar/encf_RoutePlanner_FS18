using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
     public partial class CitiesTest
    {
        [TestMethod]
        public void TestTraceLogIsImplemented()
        {
            Assert.IsTrue(TestHelpers.CheckForMethodCalledInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "TraceInformation"));
            Assert.IsTrue(TestHelpers.CheckForMethodCalledInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "TraceEvent"));
        }

        [TestMethod]
        public void TestExceptionHandlingIsImplemented()
        {
            Assert.IsTrue(TestHelpers.CheckForMethodCalledInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "try"));
            Assert.IsTrue(TestHelpers.CheckForMethodCalledInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "catch"));
        }

    }
}
