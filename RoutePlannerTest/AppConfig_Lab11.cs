using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// Not really a unit test. Tests if the console application config exists 
    /// and contains the logging entries
    /// </summary>
    [TestClass]
    public class Lab11Test
    {
        const string AppConfigFile = "/RoutePlannerConsole/bin/Debug/RoutePlannerConsole.exe.config";

        [TestMethod]
        public void TestAppConfig()
        {
            string fileContent;

            try
            {
                var projectRootDir = GetProjectRootDir();
                var appConfigPath = projectRootDir + AppConfigFile;
                Console.WriteLine($"Current dir: {appConfigPath}");

                using (TextReader reader = new StreamReader(appConfigPath))
                {
                    // read the whole file into a string
                    fileContent = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Assert.Fail("Console App config file not found");
                return;
            }

            // now check for some entries
            Assert.IsTrue(fileContent.Contains("system.diagnostics"));
            Assert.IsTrue(fileContent.Contains("listeners"));
            Assert.IsTrue(fileContent.Contains("source name"));
            Assert.IsTrue(fileContent.Contains("System.Diagnostics.TextWriterTraceListener"));
            Assert.IsTrue(fileContent.Contains("initializeData=\"Critical\""));
        }

        private  string GetProjectRootDir()
        {
            // go dir up til you find the root planner console
            bool found = false;
            string parentDir = Directory.GetCurrentDirectory();
            while (!found)
            {
                parentDir = Directory.GetParent(parentDir).FullName;
                Console.WriteLine($"Proj dir: {parentDir}");
                if (Array.Find(Directory.GetDirectories(parentDir), s => s.Contains("RoutePlannerConsole")) != null)
                {
                    found = true;
                }
            }
            return parentDir;
        }
    }
}