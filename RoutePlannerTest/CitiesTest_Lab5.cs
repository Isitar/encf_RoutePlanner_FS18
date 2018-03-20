using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
  
    public partial class CitiesTest
    {
        [TestMethod]
        public void TestFindNeighborIsASingleLinqStatement()
        {
            Func<WayPoint, double, IEnumerable<City>> method = new Cities().FindNeighbours;
            MethodInfo methodInfo = method.Method;

            TestHelpers.CheckForSingleLineLinqUsage(methodInfo);
        }

        [TestMethod]
        public void TesIfLinqAndLambdaIsUsedInReadCities()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "Select");
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "=>");
        }
    }


}