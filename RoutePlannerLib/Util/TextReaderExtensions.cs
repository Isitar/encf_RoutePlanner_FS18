using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class TextReaderExtensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader tr, char splitChar)
        {
            string line;
            while ((line = tr.ReadLine()) != null)
            {
                yield return line.Split(splitChar);
            }
        }
    }
}
