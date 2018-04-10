using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public partial class CitiesTest
    {
        private Cities cities36 = new Cities();

       
        public void Setup6()
        {
            cities36.ReadCities(@"citiesTestDataLab3.txt");
        }

        [TestMethod]
        public void TestGetPopulationOfShortestCityNames()
        {
            // have to call it explicitely sice the class contains already a TestInitializer method
            Setup6();
            Assert.AreEqual(607271, cities36.GetPopulationOfShortestCityNames());
        }

        [TestMethod]
        public void TestTestGetPopulationOfShortestCityNamesIsASingleLinqStatement()
        {
            Func<int> method = new Cities().GetPopulationOfShortestCityNames;
            MethodInfo methodInfo = method.Method;

            TestHelpers.CheckForSingleLineLinqUsage(methodInfo);
        }

        [TestMethod]
        public void TestIfLinqAndLambdaIsUsedGetPopulationOfShortestCityNames()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Cities.cs", "GetPopulationOfShortestCityNames", "=>");
        }

    }
}
