using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public partial class TextReaderExtensionTest
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";
        private const string LinksTestFile = "linksTestDataLab4.txt";

 
        [TestMethod]
        public void TestGetSplittedLinesWithCities()
        {

            using (TextReader tr = new StreamReader(CitiesTestFile))
            {
                var citiesAsStrings = tr.GetSplittedLines('\t').ToArray();

                // just check the name of the first and last city in list
                Assert.AreEqual("Zürich", citiesAsStrings[0][0].Trim()); // name
                Assert.AreEqual("Altdorf", citiesAsStrings[citiesAsStrings.Length - 1][0].Trim()); // name
            }
        }

        [TestMethod]
        public void TestGetSplittedLinesWithLinks()
        {

            using (TextReader tr = new StreamReader(LinksTestFile))
            {
                var citiesAsStrings = tr.GetSplittedLines('\t').ToArray();

                // just check the name of the first and last city in list
                Assert.AreEqual("Zürich", citiesAsStrings[0][0].Trim()); // name
                Assert.AreEqual("Dijon", citiesAsStrings[citiesAsStrings.Length - 1][0].Trim()); // name
            }
        }

        [TestMethod]
        public void TestCorrectNamespace()
        {
            Type type = typeof(TextReaderExtensions);
            Assert.IsTrue(type.FullName.Equals("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util.TextReaderExtensions"));

        }
    }
}
