// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          TestConsole
//    FileName:         TestingTranspilerPatch.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/30/2023 6:05 PM
//    Created Date:     08/30/2023 6:05 PM
// -----------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace TestConsole.Patches;

[HarmonyPatch()]
public class TestingTranspilerPatch
{ 
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(
    IEnumerable<CodeInstruction> instructions,
    ILGenerator generator,
    MethodBase original)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Callvirt)
            {
                var x = instruction.operand as MethodInfo;
                //x.Invoke(null, new object[]{ null });
                //Console.WriteLine($"{x.GetType().Name}");
            }
            Console.WriteLine($"{instruction.opcode} {instruction.operand?.ToString()} [{instruction.labels.Count}]");
            yield return instruction;
        }
    }
    
}