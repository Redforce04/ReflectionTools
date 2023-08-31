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

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InheritsClassAttribute : Attribute
{
    public InheritsClassAttribute(Type type)
    {
        _classType = type;
        isSet = true;
    }

    public InheritsClassAttribute(Type type, string method)
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
        _classType = (Type)_getterMethod!.Invoke(null, Array.Empty<object>());
        if (_classType is null)
        {
            throw new ArgumentNullException("", "Method result is null. Type is null.");
        }
        isSet = true;
    }
    
    private MethodInfo _getterMethod; 
    private bool isSet = false;
    private Type _classType;

    public Type ClassType
    {
        get
        {
            if (!isSet)
                ProcessType();
            
            return _classType;
        }
        set
        {
            _classType = value;
            isSet = true;
        }
    }
}