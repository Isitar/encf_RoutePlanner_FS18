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
    public partial class LinksTest 
    {
        private Cities cities3 = new Cities();
        private Links links3;
        private Cities cities4 = new Cities();
        private Links links4;

        
        public void Setup6()
        {
            cities3.ReadCities(@"citiesTestDataLab3.txt");
            links3 = new Links(cities3);
            links3.ReadLinks(@"linksTestDataLab3.txt");

            cities4.ReadCities(@"citiesTestDataLab4.txt");
            links4 = new Links(cities4);
            links4.ReadLinks(@"linksTestDataLab4.txt");
        }
        [TestMethod]
        public void TestGetCountOfThreeBiggestCitiesInLinks()
        {
            // have to call it explicitely sice the class contains already a TestInitializer method
            Setup6();
            Assert.AreEqual(2, links3.GetCountOfThreeBiggestCitiesInLinks());
            Assert.AreEqual(1, links4.GetCountOfThreeBiggestCitiesInLinks());

        }

        [TestMethod]
        public void TestGetCountOfThreeCitiesWithLongestNameinLinks()
        {
            // have to call it explicitely sice the class contains already a TestInitializer method
            Setup6();
            Assert.AreEqual(3, links3.GetCountOfThreeCitiesWithLongestNameInLinks());
            Assert.AreEqual(5, links4.GetCountOfThreeCitiesWithLongestNameInLinks());

        }

        [TestMethod]
        public void TestTestGetCountOfThreeCitiesWithLongestNameinLinksIsASingleLinqStatement()
        {
            Func<int> method = new Links(null).GetCountOfThreeCitiesWithLongestNameInLinks;
            MethodInfo methodInfo = method.Method;

            TestHelpers.CheckForSingleLineLinqUsage(methodInfo);
        }

        [TestMethod]
        public void TestIfLinqAndLambdaIsUsedGetCountOfThreeCitiesWithLongestNameInLinks()
        {
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Links.cs", "GetCountOfThreeCitiesWithLongestNameInLinks", "Count");
            TestHelpers.CheckForMethodCallInMethod("../../../RoutePlannerLib/Links.cs", "GetCountOfThreeCitiesWithLongestNameInLinks", "=>");
        }

    }
}
