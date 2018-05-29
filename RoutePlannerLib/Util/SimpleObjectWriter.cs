using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        private readonly TextWriter tw;

        public SimpleObjectWriter(TextWriter tw)
        {
            this.tw = tw;
        }

        public void Next(object c)
        {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var type = c.GetType();
            tw.WriteLine($"Instance of {type.FullName}");
            foreach (var t in type.GetProperties().OrderBy(p => p.Name))
            {
                if (t.PropertyType == typeof(string))
                {
                    tw.WriteLine($"{t.Name}=\"{t.GetValue(c)}\"");
                }

                if (new[] { typeof(int), typeof(float), typeof(double) }.Contains(t.PropertyType))
                {
                    tw.WriteLine($"{t.Name}={t.GetValue(c)}");
                }
                if (t.PropertyType.IsClass && t.PropertyType != typeof(String))
                {
                    tw.WriteLine($"{t.Name} is a nested object...");
                    Next(t.GetValue(c));
                }
            }

            tw.WriteLine("End of instance");
            Thread.CurrentThread.CurrentCulture = origCulture;
        }
    }
}