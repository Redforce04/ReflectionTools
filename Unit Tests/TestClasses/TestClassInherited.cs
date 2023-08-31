// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          Unit Tests
//    FileName:         TestClassInherited.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/29/2023 11:56 AM
//    Created Date:     08/29/2023 11:56 AM
// -----------------------------------------

using System;

namespace Unit_Tests.TestClasses;

public class TestClassInherited : ITestInterface
{
    public string Test { get; }
    public string Test2 { get; }
    public string Test3 { get; }
    public void Test4()
    {
        Console.WriteLine("Test4");
    }

    public void Test5()
    {
        Console.WriteLine("Test5");
    }
}