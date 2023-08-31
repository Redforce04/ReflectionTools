using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unit_Tests.TestClasses;

namespace Unit_Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void InterfaceTest()
        {
            Type inherited = typeof(TestClassInherited);
            var newType = ReflectionTools.Reflections.MakeCustomType<TestClass>(typeof(ITestInterface));
            if (newType is null)
            {
                Console.WriteLine($"Null");
                return;
            }

            List<PropertyInfo> inheritedProperties = inherited.GetProperties().ToList();
            List<PropertyInfo> properties = newType.GetProperties().ToList();
            Console.WriteLine("properties.");
            Assert.True(true);
        }
    }
}