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
   
    public partial class CitiesTest
    {
        [TestMethod]
        public void TestIfSplittedLineMethodIsCalled()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Cities.cs", "ReadCities", "GetSplittedLines");
        }

       
    }
}
