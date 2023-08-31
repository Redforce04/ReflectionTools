// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          Unit Tests
//    FileName:         TestClass.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/29/2023 11:56 AM
//    Created Date:     08/29/2023 11:56 AM
// -----------------------------------------

using System;
using System.Linq;
using System.Reflection;
using ReflectionTools.Attributes;

namespace TestConsole.TestClasses;

[IsInterface(typeof(TestClass), nameof(GetInterfaceType))]
[InheritsClass(typeof(TestClass), nameof(GetAbstractClassType))]
public class TestClass
{
    public static Type GetInterfaceType()
    {
        var firstOrDefault = typeof(TestClass).Assembly.DefinedTypes.First(x => x.Name == "ITestInterface");
        if (firstOrDefault is null)
        {
            Console.WriteLine("Interface is null");
        }

        return firstOrDefault;
    }

    public static Type GetAbstractClassType()
    {
        var firstOrDefault = typeof(TestClass).Assembly.DefinedTypes.First(
            x => x.Name == "AbstractClass");
        if (firstOrDefault is null)
        {
            Console.WriteLine("Interface is null");
        }

        return firstOrDefault;
    }

    public string Test { get; }
    public string Test2 { get; }
    public string Test3 { get; }
    public string Test6;
    [CallMethod(typeof(TestClass),nameof(Test4Execute))]
    public void Test4()
    {
        Test4Execute(this);
    }

    public static bool Test4Execute(object __instance)
    {
        Console.WriteLine("Test4 executed");
        return false;
    }

    [CallMethod(typeof(TestClass),nameof(Test5Execute))]
    public void Test5()
    {
        Test5Execute(this);
    }

    public static bool Test5Execute(object __instance)
    {
        Console.WriteLine("Test5 executed");
        return false;
    }
}