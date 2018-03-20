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
    public partial class LinksTest
    {
        [TestMethod]
        public void TestFindCitiesByTransportMode()
        {
            Cities cities = new Cities();
            cities.ReadCities(@"citiesTestDataLab3.txt");
            var routes = new Links(cities);

            // run on empty lists
            City[] emptyCitiesByMode = routes.FindCities(TransportMode.Bus);
            Assert.AreEqual(0, emptyCitiesByMode.Length);

            // now read links
            routes.ReadLinks(@"linksTestDataLab3.txt");

            // run tests on non empty lists
            City[] citiesByMode = routes.FindCities(TransportMode.Rail);
            Assert.AreEqual(11, citiesByMode.Length);

            emptyCitiesByMode = routes.FindCities(TransportMode.Bus);
            Assert.AreEqual(0, emptyCitiesByMode.Length);

            emptyCitiesByMode = routes.FindCities((TransportMode)99);
            Assert.AreEqual(0, emptyCitiesByMode.Length);
        }

        [TestMethod]
        public void TestFindCitiesByTransportModeIsASingleLinqStatement()
        {
            Func<TransportMode,City[]> method = new Links(null).FindCities;
            MethodInfo methodInfo = method.Method;

            TestHelpers.CheckForSingleLineLinqUsage(methodInfo);
        }

       
    }
}