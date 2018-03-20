using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    /// <summary>
    /// A test helper class which contains generic test methods which are used for concrete
    /// test during the labs.
    /// </summary>
    [TestClass]
    class TestHelpers
    {
        /// <summary>
        /// Checks if the methods of the passed type conform to the Microsoft coding convention
        /// that methods start with a capital letter.
        /// </summary>
        /// <param name="type"></param>
        [TestMethod]
        public static void CheckMethodNamesAreCapitalized(Type type)
        {
            var methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var methodInfo in methodInfos)
            {
                var name = methodInfo.Name.Replace("get_", string.Empty).Replace("set_", string.Empty).Replace("<", string.Empty);
                Assert.IsTrue(char.IsUpper(name.First()), $"Method {name} on class {type.Name} does not start with upper case.");
            }
        }

        public static void CheckForMethodCallInMethod(string filename, string callingMethod, string calledMethod)
        {
            using (TextReader reader = new StreamReader(filename))
            {
                string sourceCode = reader.ReadToEnd();
                string method = GetMethodFromCode(sourceCode, callingMethod);
                Assert.IsTrue(method.Contains(calledMethod));

            }
        }

        public static string GetMethodFromCode(string code, string methodName)
        {
            string method;

            method = code.Substring(code.IndexOf(methodName));
            method = method.Substring(0, FindIndexOfMethodEnd(method));

            return method;
        }

        static int FindIndexOfMethodEnd(string methodBegin)
        {
            int startIndex = methodBegin.IndexOf('{');
            int openBraces = 0;

            for (int i = startIndex; i < methodBegin.Length; i++)
            {
                if (methodBegin[i] == '{') openBraces++;
                if (methodBegin[i] == '}') openBraces--;

                if (openBraces == 0) return i+1;

            }

            return methodBegin.Length;
        }

        /// <summary>
        /// Simple test if a method is a single liner making a LINQ call (opcode 40)
        /// </summary>
        /// <param name="methodInfo">the method to check</param>
        public static void CheckForSingleLineLinqUsage(MethodInfo methodInfo)
        {
            Assert.IsTrue(methodInfo.GetMethodBody().LocalVariables.Count <= 2,
                "Implement the method FindCities as a single-line LINQ statement in the form \"return [LINQ];\".");

            // some more not very sophisticated tests to ensure LINQ has been used
            MethodBody mb = methodInfo.GetMethodBody();
            // the method should be smaller than 100 IL byte instructions
            Assert.IsTrue(mb.GetILAsByteArray().Length < 100);
            // and it should contain two "call" ops (to LINQ)
            Assert.IsTrue(mb.GetILAsByteArray().ToList().Contains(40));
            //TODO: verify that a call to LINQ method is done
        }
    }
}
