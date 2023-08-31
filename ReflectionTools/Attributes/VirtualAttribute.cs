// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         Virtual.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/31/2023 11:51 AM
//    Created Date:     08/31/2023 11:51 AM
// -----------------------------------------

using System;

namespace ReflectionTools.Attributes;
[AttributeUsage((AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property))]
public class Virtual : Attribute
{
    
}