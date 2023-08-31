// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         Loader.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/31/2023 11:26 AM
//    Created Date:     08/31/2023 11:26 AM
// -----------------------------------------

namespace ReflectionTools;

internal class Loader
{
    internal static bool IsLoaded = false;

    internal static void Load()
    {
        if (IsLoaded)
        {
            return;
        }
        CosturaUtility.Initialize();
        IsLoaded = true;
    }
}