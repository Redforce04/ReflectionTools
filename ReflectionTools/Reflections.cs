using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ReflectionTools.Attributes;

namespace ReflectionTools
{
    public class Reflections
    {
        private const bool Interface = true;

        /// <summary>
        /// Makes a type that is an interface of an object.
        /// </summary>
        /// <param name="interfaceType">The Interface's Type</param>
        /// <typeparam name="T">The pre-existing type that you want to inherit the interface</typeparam>
        /// <returns>A new type with the same properties and methods that inherits the interface</returns>
        public static Type? MakeCustomType<T>()
        {
            // Load loader (harmony)
            if (!Loader.IsLoaded)
            {
                Loader.Load();
            }
            var typeBuilder = new CustomTypeBuilder(typeof(T));
            return typeBuilder.Result;
        }


    }

    public static class Extensions
    {
        public static string? FirstCharToLowerCase(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str![0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str.Substring(1);

            return str;
        }

        public static bool IsVirtual(this object obj)
        {
            if (obj is FieldInfo fieldInfo)
            {
                return fieldInfo.GetCustomAttribute(typeof(Virtual)) is null;
            }

            if (obj is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetCustomAttribute(typeof(Virtual)) is null;
            }

            if (obj is MethodInfo methodInfo)
            {
                return methodInfo.GetCustomAttribute(typeof(Virtual)) is null;
            }

            return false;
        }
    }
}
