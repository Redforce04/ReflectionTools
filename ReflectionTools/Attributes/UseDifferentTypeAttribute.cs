// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         InterfaceAttribute.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/31/2023 11:34 AM
//    Created Date:     08/31/2023 11:34 AM
// -----------------------------------------

using System;
using System.Reflection;

namespace ReflectionTools.Attributes;

public class UseDifferentTypeAttribute : Attribute
{
    public UseDifferentTypeAttribute(Type type, string methodName)
    {
        GetterMethod = type.GetMethod(methodName) ?? throw new ArgumentNullException("methodName", "Method is not a valid method.");
        if (GetterMethod.GetParameters().Length > 0 || !GetterMethod.IsStatic)
        {
            throw new ArgumentException("The method must not require any parameters and must be static.");
        }
    }

    public void GetMethodValue()
    {
        _value = GetterMethod.Invoke(null, Array.Empty<object>());
        _isSet = true;
    }
    

    public MethodInfo GetterMethod;
    private object _value;
    private bool _isSet = false;

    public object Value
    {
        get
        {
            if (!_isSet)
                GetMethodValue();
            
            return _value;
        }
        set
        {
            _value = value;
            _isSet = true;
        }
    }
}