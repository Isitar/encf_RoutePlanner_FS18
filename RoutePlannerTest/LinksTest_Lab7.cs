using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public partial class LinksTest
    {
        private const string CitiesTestFile7 = "citiesTestDataLab4.txt";
        private const string LinksTestFile7 = "linksTestDataLab4.txt";

        /// <summary>
        /// Tests if synchronous and asynchronous execution provide the same results.
        /// </summary>
        [TestMethod]
        public async Task TestFindShortestRouteBetweenAsync()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile7);

            var routes = new Links(cities);
            routes.ReadLinks(LinksTestFile7);

            // do synchronous execution
            var linksExpected = routes.FindShortestRouteBetween("Basel", "Zürich", TransportMode.Rail);

            // do asynchronous execution
            var linksActual = await routes.FindShortestRouteBetweenAsync("Basel", "Zürich", TransportMode.Rail);

            // now test the results
            Assert.IsNotNull(linksActual);
            Assert.AreEqual(linksExpected.Count, linksActual.Count);

            for (int i = 0; i < linksActual.Count; i++)
            {
                Assert.AreEqual(linksExpected[i].FromCity, linksActual[i].FromCity);
                Assert.AreEqual(linksExpected[i].ToCity, linksActual[i].ToCity);
            }
        }

        /// <summary>
        /// System.Progress does fancy asynchronous re-scheduling of progress reports, which
        /// cannot not work reliably in unit tests. This class synchronously calls the provided
        /// action callback.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class DirectProgressCallback<T>:IProgress<T>
        {
            private Action<T> callback;
            internal DirectProgressCallback(Action<T> _callback)
			{
				callback = _callback;
			}
            public void Report(T value) => callback(value);
        }
		
		/// <summary>
        /// Tests whether progress reports are sent.
        /// </summary>
        [TestMethod]
        public async Task TestFindShortestRouteBetweenAsyncProgress()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile7);

            var routes = new Links(cities);
            routes.ReadLinks(LinksTestFile7);

            // do synchronous execution
            var linksExpected = routes.FindShortestRouteBetween("Basel", "Zürich", TransportMode.Rail);

            // do asynchronous execution
            var messages = new List<string>();
            var progress = new DirectProgressCallback<string>(msg => messages.Add(msg));
            var linksActual = await routes.FindShortestRouteBetweenAsync("Basel", "Zürich", TransportMode.Rail, progress);

            // ensure that at least 5 progress calls are made
            Assert.IsTrue(messages.Distinct().Count()>=5, "Less than 5 distinct progress messages");

            // ensure that all progress messages end with " done"
            Assert.IsTrue(messages.All(m => m.EndsWith(" done")),
                string.Format("Progress message \"{0}\" does not end with \" done\"",
                    messages.FirstOrDefault(m => !m.EndsWith(" done"))));
        }
    }
}
