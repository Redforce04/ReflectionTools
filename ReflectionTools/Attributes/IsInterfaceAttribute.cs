// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         IsInterfaceAttribute.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/31/2023 12:06 PM
//    Created Date:     08/31/2023 12:06 PM
// -----------------------------------------

using System;
using System.Reflection;

namespace ReflectionTools.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class IsInterfaceAttribute : Attribute
{
    public IsInterfaceAttribute(Type type)
    {
        _interfaceType = type;
        isSet = true;
    }

    public IsInterfaceAttribute(Type type, string method)
    {
        var getterMethod = type.GetMethod(method);
        if (getterMethod is null)
        {
            throw new ArgumentNullException("method");
        }

        if (getterMethod.GetParameters().Length > 0 || getterMethod.ReturnType != typeof(Type) || !getterMethod.IsStatic)
        {
            throw new ArgumentException("Getter method must return a Type, be static, and have no args.", "method");
        }

        _getterMethod = getterMethod;
    }

    private void ProcessType()
    {
        _interfaceType = (Type)_getterMethod!.Invoke(null, Array.Empty<object>());
        if (_interfaceType is null)
        {
            throw new ArgumentNullException("", "Method result is null. Type is null.");
        }
        isSet = true;
    }
    
    private MethodInfo _getterMethod; 
    private bool isSet = false;
    private Type _interfaceType;

    public Type InterfaceType
    {
        get
        {
            if (!isSet)
                ProcessType();
            
            return _interfaceType;
        }
        set
        {
            _interfaceType = value;
            isSet = true;
        }
    }
}