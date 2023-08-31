// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         CallMethodAttribute.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/30/2023 4:14 PM
//    Created Date:     08/30/2023 4:14 PM
// -----------------------------------------

using System;
using System.Linq;
using System.Reflection;

namespace ReflectionTools.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CallMethodAttribute : Attribute
{
    public CallMethodAttribute(Type type, string methodName, bool includeObjectAtStart = true)
    {
        var method2 = type.GetMethod(methodName);
        if (method2 is null)
        {
            throw new MissingMethodException($"Could not find method \"{methodName}\"");
        }

        MethodInfo = method2;
        VerifyMethodIsValid();
    }

    private void VerifyMethodIsValid()
    {
        if (!MethodInfo.IsStatic)
        {
            throw new ArgumentOutOfRangeException("MethodInfo","The method must be static, and public. The instance can be used as the first variable.");
        }

        if (!MethodInfo.IsPublic)
        {
            throw new ArgumentOutOfRangeException("MethodInfo","The method must be static, and public. The instance can be used as the first variable.");
        }
    }

    public bool IncludeObjectAtStart { get; set; } = true;
    public MethodInfo MethodInfo { get; set; }

}