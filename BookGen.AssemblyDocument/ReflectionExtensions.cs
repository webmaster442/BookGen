using BookGen.AssemblyDocument.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BookGen.AssemblyDocument
{
    public static class ReflectionExtensions
    {
        public static TypeType GetTypeType(this Type type)
        {
            if (type.IsInterface)
                return TypeType.Interface;
            else if (IsRecord(type))
                return TypeType.Record;
            else if (typeof(Delegate).IsAssignableFrom(type))
                return TypeType.Delegate;
            else if (type.IsClass)
                return TypeType.Class;
            else if (type.IsEnum)
                return TypeType.Enum;
            else if (type.IsValueType)
                return TypeType.Struct;
            else
                return TypeType.Unknown;
        }

        public static string GetTypeArgumentString(this Type type)
        {
            if (type.IsGenericType)
                return $"<{string.Join(", ", type.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters.Select(x => x.Name))}>";
            return string.Empty;
        }

        public static IEnumerable<Type> GetInheritanceChain(this Type type)
        {
            Type? cycle = type.BaseType;
            while (cycle != null)
            {
                yield return cycle;
                cycle = cycle.BaseType;
            }
        }

        public static string GetTypeSelectorName(this Type type)
        {
            return $"T:{type.FullName}";
        }

        private static bool IsRecord(Type type)
        {
            var check1 = type
                .GetTypeInfo()
                .DeclaredProperties
                .FirstOrDefault(x => x.Name == "EqualityContract")?
                .GetMethod?
                .GetCustomAttribute(typeof(CompilerGeneratedAttribute)) is object;

            var check2 = type.GetMethod("<Clone>$") is object;

            return check1 && check2;
        }
    }
}
