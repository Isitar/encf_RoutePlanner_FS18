using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
     public partial class LinksTest
    {
        [TestMethod]
        public void TestTraceLogIsImplemented()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Links.cs", "ReadLinks", "TraceInformation");
        }

    }
}
