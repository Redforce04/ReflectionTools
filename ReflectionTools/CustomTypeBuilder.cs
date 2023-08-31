// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ReflectionTools
//    Project:          ReflectionTools
//    FileName:         CustomTypeBuilder.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/31/2023 1:03 PM
//    Created Date:     08/31/2023 1:03 PM
// -----------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ReflectionTools;
using ReflectionTools.Attributes;

namespace ReflectionTools;
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
public class CustomTypeBuilder
{
    private static Lazy<HarmonyLib.Harmony> harmony = new Lazy<HarmonyLib.Harmony>(() => new HarmonyLib.Harmony("me.redforce04.reflectiontools"));
    public CustomTypeBuilder(Type type)
    {
        baseType = type;
        Run();
    }

    public Type Result;
    private Type baseType;
    private TypeBuilder typeBuilder; 
    private Dictionary<string, MethodInfo> patches = new Dictionary<string, MethodInfo>();
    public void Run()
    {
        // Get Interfaces that are present.
        List<Type> interfaces = new List<Type>();
        Type? abstractClass = baseType.GetCustomAttribute<InheritsClassAttribute>().ClassType;

        foreach (IsInterfaceAttribute attribute in baseType.GetCustomAttributes<IsInterfaceAttribute>())
        {
            if (!attribute.InterfaceType.IsInterface && !attribute.InterfaceType.IsAbstract)
            {
                Console.WriteLine($"Not interface, skipping.");
                continue;
            }

            interfaces.Add(attribute.InterfaceType);
        }
        if (!abstractClass.IsInterface && !abstractClass.IsAbstract)
        {
            Console.WriteLine($"Not abstract class, skipping.");
            abstractClass = null;
        }



        var assemblyName = new AssemblyName("temp");
        var appDomain = System.Threading.Thread.GetDomain();
        var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
        typeBuilder = moduleBuilder.DefineType($"{baseType.Name}OfInterface",
            TypeAttributes.Public | TypeAttributes.Class, abstractClass);
        
        foreach (Type interfaceType in interfaces)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);
        }

        object? type = null;
        try
        {
            type = baseType.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>());
        }
        catch (Exception)
        {
            // unused
        }

        foreach (FieldInfo field in baseType.GetFields())
        {
            ProcessField(ref type, field);
        }

        foreach (PropertyInfo property in baseType.GetProperties())
        {
            ProcessProperty(ref type, property);
        }

        foreach (MethodInfo method in baseType.GetMethods())
        {
            ProcessMethod(method);
        }

        if (abstractClass != null) interfaces.Add(abstractClass);
        // Foreach Interface - 
        foreach (Type interfaceType in interfaces)
        {
            foreach (PropertyInfo property in interfaceType.GetProperties())
            {
                if ((property.GetMethod?.IsStatic ?? false) || (property.SetMethod?.IsStatic ?? false))
                {
                    continue;
                }

                if (!(property.GetMethod?.IsVirtual ?? false) && !(property.SetMethod?.IsVirtual ?? false) &&
                    !(property.GetMethod?.IsAbstract ?? false) && !(property.SetMethod?.IsAbstract ?? false))
                {
                    continue;
                }

                Console.WriteLine($"Required Interface Property: {property.Name}");
                /*if (!typeBuilder.DeclaredProperties.Any(x => x.Name == property.Name))
                {
                    ProcessProperty(ref type, property);
                }*/
            }

            // process required interface methods.
            foreach (MethodInfo method in interfaceType.GetMethods())
            {
                if (method.IsStatic || !method.IsAbstract && !method.IsVirtual || method.IsSpecialName)
                {
                    continue;
                }
                
                Console.WriteLine($"Required Interface Method: {method.Name}");
                /*if (!typeBuilder.DeclaredMethods.Any(x => x.Name == method.Name))
                {
                    ProcessMethod(method);
                }*/
            }
        }

        var newType = typeBuilder.CreateType();
        foreach (var patch in patches)
        {
            MethodBase? @base = newType.GetMethod(patch.Key);
            if (@base is null)
            {
                Console.WriteLine($"Warning: Patch is null.");
                continue;
            }

            // HarmonyPrefix prefix = new HarmonyPrefix();
            HarmonyLib.HarmonyMethod method = new HarmonyLib.HarmonyMethod(patch.Value);
            harmony.Value.Patch(@base, method);
        }

        this.Result = newType;
    }

    private void ProcessField(ref object obj, FieldInfo field)
        {
            if (field.IsStatic)
            {
                return;
            }
            
            string fieldName = field.Name;
            Type type = field.FieldType;
            FieldAttributes fieldAttributes = field.Attributes;
            typeBuilder.DefineField(fieldName, type, fieldAttributes);
        }
        private void ProcessMethod(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<CallMethodAttribute>();
            if (methodInfo.IsStatic || attribute is null || methodInfo.IsSpecialName)
            {
                return;
            }

            bool isVirtual = methodInfo.IsVirtual();
            MethodInfo replacement = attribute.MethodInfo;

            CallingConventions callingConventions = methodInfo.CallingConvention;
            List<Type> parameters = new List<Type>();
            string parameterList = "[ ";
            bool first = true;
            bool useObjectInstance = false;
            foreach (var obj in attribute.MethodInfo.GetParameters())
            {
                if (first)
                {
                    first = false;
                    if (obj.ParameterType == typeof(object))
                    {
                        useObjectInstance = true;
                        continue;
                    }
                }

                parameterList += $"{obj.Name} ({obj.ParameterType}), ";
                parameters.Add(obj.ParameterType);
            }
            
            var methodAttributes = methodInfo.Attributes;
            if (isVirtual)
                methodAttributes = methodAttributes | MethodAttributes.Final | MethodAttributes.Virtual;
            
            MethodBuilder builder = typeBuilder.DefineMethod(
                methodInfo.Name, 
                methodAttributes, 
                callingConventions,
                methodInfo.ReturnType, 
                parameters.ToArray());
            ILGenerator ilGenerator = builder.GetILGenerator();
            Console.WriteLine($"Ret");
                ilGenerator.Emit(OpCodes.Ret);
                patches.Add(methodInfo.Name, replacement);
        }

        private void ProcessProperty(ref object? type, PropertyInfo property)
        {
            object? value = null;
            if (type is not null)
            {
                value = property.GetValue(type);
            }

            bool isVirtual = property.IsVirtual();

            PropertyAttributes propertyAttributes =
                (value is null ? PropertyAttributes.None : PropertyAttributes.HasDefault);
            Type[]? typeParams = null; //Type.EmptyTypes;
            bool hasThis = (property.GetGetMethod().CallingConvention.HasFlag(CallingConventions.HasThis) ||
                            property.GetSetMethod().CallingConvention.HasFlag(CallingConventions.HasThis));
            CallingConventions callingConventions =
                hasThis ? CallingConventions.Standard | CallingConventions.HasThis : CallingConventions.Any;

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(property.Name, propertyAttributes,
                callingConventions, property.PropertyType, typeParams);

            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + $"{property.Name.FirstCharToLowerCase()}",
                property.PropertyType, FieldAttributes.Private | FieldAttributes.SpecialName);


        // Last, we must map the two methods created above to our PropertyBuilder to
            // their corresponding behaviors, "get" and "set" respectively.
            var getter = property.GetGetMethod();
            if (getter is not null)
            {
                MethodBuilder getterBuilder = DefineGetterSetter(ref fieldBuilder, property, getter, hasThis, isVirtual, true);
                propertyBuilder.SetGetMethod(getterBuilder);
            }

            var setter = property.GetSetMethod();
            if (setter is not null)
            {
                MethodBuilder setterBuilder = DefineGetterSetter(ref fieldBuilder, property, setter, hasThis, isVirtual, false);
                propertyBuilder.SetSetMethod(setterBuilder);
            }
        }

        private MethodBuilder DefineGetterSetter(
            ref FieldBuilder fieldBuilder,
            PropertyInfo property,
            MethodInfo originalGetter,
            bool hasThis, 
            bool isVirtual,
            bool isGetter)
        {
            var methodAttributes = originalGetter.Attributes | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            if (isVirtual)
                methodAttributes = methodAttributes | MethodAttributes.Final | MethodAttributes.Virtual;
                
            // Define the "get" accessor method for CustomerName.
            MethodBuilder methodBuilder =
                typeBuilder.DefineMethod($"{(isGetter ? "get" : "set")}_{property.Name}",
                    methodAttributes,
                    (hasThis ? CallingConventions.Standard | CallingConventions.HasThis : CallingConventions.Any),
                    isGetter ? property.PropertyType : null,
                    isGetter ? Type.EmptyTypes : new Type[]{ property.PropertyType });
            
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            if(isGetter)
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            else {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            }
            ilGenerator.Emit(OpCodes.Ret);
            return methodBuilder;
        }

}

internal enum MethodType
{
   Getter,
   Setter,
   Normal
}
