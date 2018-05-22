using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export;
using System.IO;
using System.Net.Mime;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    public class ExcelExchangeTest
    {
        [TestMethod]
        public void TestExcelExport()
        {
            var excelFileName = Directory.GetCurrentDirectory() + @"\ExportTest.xlsx";
            Console.WriteLine($"Excel out dir: {excelFileName}");
            var bern = new City("Bern", "Switzerland", 5000, 46.95, 7.44);
            var zuerich = new City("Zürich", "Switzerland", 100000, 32.876174, 13.187507);
            var aarau = new City("Aarau", "Switzerland", 10000, 35.876174, 12.187507);
            var links = new Link[]
            {
                new Link(bern, aarau, 15, TransportMode.Ship),
                new Link(aarau, zuerich, 20, TransportMode.Ship)
            };

            var excel = new ExcelExchange();
            excel.WriteToFile(excelFileName, links);
            Assert.IsTrue(File.Exists(excelFileName), excelFileName);

            //should not show dialog boxes or fail, should silently overwrite the file
            excel.WriteToFile(excelFileName, links);
            Assert.IsTrue(File.Exists(excelFileName), excelFileName);

            // check that data are really there
            CheckFileContent(excelFileName, links);

        }

        /// <summary>
        /// helper method to check also the file content
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="links"></param>
        public void CheckFileContent(string fileName, IEnumerable<Link> links)
        {

            // start excel and open existing workbook
            var excel = StartExcelApp();
            Workbook workbook = excel.Workbooks.Open(fileName);
            var sheet = (Worksheet)workbook.ActiveSheet;

            // check formatting of header and data cells content
            CheckHeaderCells(sheet);

            CheckDataCells(links, sheet);

            workbook.Close(false, fileName, System.Reflection.Missing.Value);
            excel.Quit();
            excel = null;

        }

        private static Application StartExcelApp()
        {
            var excel = new Application();
            excel.Visible = false;
            excel.ScreenUpdating = false;

            excel.DisplayAlerts = false;
            return excel;
        }

        private static void CheckHeaderCells(Worksheet sheet)
        {
            var header = sheet.Range["A1", "D1"];

            Assert.IsTrue(header.Font.Bold);
            Assert.AreEqual(14, header.Font.Size);
            Assert.AreEqual("From", sheet.Cells[1, 1].Value.ToString());
        }

        private static void CheckDataCells(IEnumerable<Link> links, Worksheet sheet)
        {
            int i = 2;
            foreach (Link l in links)
            {
                // just check the 
                Assert.AreEqual(l.FromCity.Name + " (" + l.FromCity.Country + ")", sheet.Cells[i, 1].Value.ToString());
                Assert.AreEqual(l.ToCity.Name + " (" + l.ToCity.Country + ")", sheet.Cells[i, 2].Value.ToString());
                Assert.AreEqual(l.Distance, Convert.ToDouble(sheet.Cells[i, 3].Value.ToString()), 0.0001);
                Assert.AreEqual(l.TransportMode.ToString(), sheet.Cells[i, 4].Value.ToString());

                i++;
            }
        }

    }
}
