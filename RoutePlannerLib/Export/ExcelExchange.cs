using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    public class ExcelExchange
    {
        public void WriteToFile(string fileName, IEnumerable<Link> links)
        {
            var excelApp = new Application();
            excelApp.Visible = true;
            
            var workbook = (Microsoft.Office.Interop.Excel._Workbook)(excelApp.Workbooks.Add(""));
            var sheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

            //Add table headers going cell by cell.
            sheet.Cells[1, 1].Value = "From";
            sheet.Cells[1, 2].Value = "To";
            sheet.Cells[1, 3].Value = "Distance";
            sheet.Cells[1, 4].Value = "Mode";
            
            // add lines
            var linkArr = links as Link[] ?? links.ToArray();
            for (int i = 0; i < linkArr.Count(); i++)
            {
                var l = linkArr[i];
                sheet.Cells[i + 2, 1] = l.FromCity.Name + " (" + l.FromCity.Country + ")";
                sheet.Cells[i + 2, 2] = l.ToCity.Name + " (" + l.ToCity.Country + ")";
                sheet.Cells[i + 2, 3] = l.Distance;
                sheet.Cells[i + 2, 4] = l.TransportMode.ToString();
            }


            var header = sheet.Range["A1", "D1"];
            sheet.Range["A1", "D1"].Font.Bold = true;
            sheet.Range["A1", "D1"].Font.Size = 14;

            workbook.SaveAs(fileName, XlFileFormat.xlWorkbookDefault,Type.Missing,Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges,Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workbook.Close();
            excelApp.Quit();
        }
    }
}
