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
    public class SimpleObjectReader
    {
        private const string ObjectStarter = "Instance of ";
        private const string ObjectStopper = "End of instance";
        private const string ObjectHint = " is a nested object...";
        private readonly TextReader tr;


        public SimpleObjectReader(TextReader tr)
        {
            this.tr = tr;
        }

        private Assembly FindAssembly(string classname) => AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetTypes().Select(t => t.FullName).Contains(classname));
       
        public object Next()
        {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            if (tr.Peek() == -1) return null;
            bool eoo = false;
            object retObject = null;
            var currAssembly = Assembly.GetExecutingAssembly();
            var className = "";
            while (!eoo)
            {
                var line = tr.ReadLine();
                if (line == null || line.Equals(ObjectStopper))
                {
                    eoo = true;
                    continue;
                }

                if (retObject == null)
                {
                    if (!line.StartsWith(ObjectStarter))
                    {
                        throw new ArgumentException($"illegal file content, starts with {line}");
                    }
                    className = line.Substring(ObjectStarter.Length);
                    currAssembly = FindAssembly(className);
                    retObject = currAssembly.CreateInstance(className);
                }
                else
                {
                    if (line.EndsWith(ObjectHint))
                    {
                        var propertyName = line.Substring(0, line.Length - ObjectHint.Length);
                        currAssembly.GetType(className).GetProperty(propertyName)?.SetValue(retObject, Next());
                    }
                    else
                    {
                        var splittedLine = line.Split('=');
                        var propertyName = splittedLine[0];
                        var propertyValue = splittedLine[1];
                        if (propertyValue.StartsWith("\""))
                        {
                            propertyValue = propertyValue.Substring(1, propertyValue.Length - 2);
                        }

                        var property = currAssembly.GetType(className).GetProperty(propertyName);
                        property?.SetValue(retObject, Convert.ChangeType(propertyValue, property.PropertyType));
                    }
                }
            }
            Thread.CurrentThread.CurrentCulture = origCulture;
            return retObject;
        }

    }
}
