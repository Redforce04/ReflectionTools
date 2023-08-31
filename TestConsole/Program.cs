using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TestConsole.Patches;
using TestConsole.TestClasses;

namespace TestConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Harmony harmony = new Harmony("testing");
            
            Type inherited = typeof(TestClassInherited);
            var newType = ReflectionTools.Reflections.MakeCustomType<TestClass>();
            if (newType is null)
            {
                Console.WriteLine($"Null");
                return;
            }

            List<FieldInfo> inheritedFields = inherited.GetFields().ToList();
            List<FieldInfo> fields = newType.GetFields().ToList();
            Console.WriteLine("fields.");

            List<PropertyInfo> inheritedProperties = inherited.GetProperties().ToList();
            List<PropertyInfo> properties = newType.GetProperties().ToList();
            Console.WriteLine("properties.");
            
            object methodsProper = inherited.GetMethods();
            object methods = newType.GetMethods();
            Console.WriteLine("methods.");
            
            object instanceProper = new TestClassInherited();
            object instance = newType.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>());
            Console.WriteLine("instances.\n\n");
            
            var Test4 = newType.GetMethod("Test4");
            // var method = new HarmonyMethod(TestingTranspilerPatch.Transpiler);
            // harmony.Patch(Test4, transpiler: method);
            Test4.Invoke(instance, Array.Empty<object>());
            
            /*
            var Test5 = newType.GetMethod("Test5");
            Test5.Invoke(instance, Array.Empty<object>());
            */
            Console.WriteLine("invokes.");
        }
    }
}