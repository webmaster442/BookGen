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

        public static string GetTypeNameForTitle(this Type type)
        {
            if (type.IsGenericType)
                return $"<{string.Join(", ", type.GetGenericTypeDefinition().GetTypeInfo().GenericTypeParameters.Select(x => x.Name))}>";
            return type.FullName ?? string.Empty;
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

        public static string GetNormalizedTypeName(this Type type)
        {
            if (type.FullName == typeof(int).FullName)
            {
                return "int";
            }
            else if (type.FullName == typeof(uint).FullName)
            {
                return "uint";
            }
            else if (type.FullName == typeof(byte).FullName)
            {
                return "byte";
            }
            else if (type.FullName == typeof(sbyte).FullName)
            {
                return "sbyte";
            }
            else if (type.FullName == typeof(long).FullName)
            {
                return "long";
            }
            else if (type.FullName == typeof(ulong).FullName)
            {
                return "ulong";
            }
            else if (type.FullName == typeof(short).FullName)
            {
                return "short";
            }
            else if (type.FullName == typeof(ushort).FullName)
            {
                return "ushort";
            }
            else if (type.FullName == typeof(float).FullName)
            {
                return "float";
            }
            else if (type.FullName == typeof(double).FullName)
            {
                return "double";
            }
            else if (type.FullName == typeof(decimal).FullName)
            {
                return "decimal";
            }
            else if (type.FullName == typeof(string).FullName)
            {
                return "string";
            }
            else
            {
                return type.FullName ?? string.Empty;
            }
        }

        public static string GetPropertyCode(this PropertyInfo property)
        {
            List<string> parts = new List<string>();
            if (property.GetMethod?.IsPublic ?? false)
            {
                parts.Add("public");
            }
            else if (property.GetMethod?.IsFamilyOrAssembly ?? false)
            {
                parts.Add("protected internal");
            }
            else if (property.GetMethod?.IsAssembly ?? false)
            {
                parts.Add("internal");
            }
            else if (property.GetMethod?.IsFamily ?? false)
            {
                parts.Add("protected");
            }
            else
            {
                parts.Add("private");
            }
            parts.Add(GetNormalizedTypeName(property.PropertyType) ?? "");
            parts.Add(property.Name);
            parts.Add("{");
            if (property.CanRead)
            {
                parts.Add("get;");
            }

            if (property.IsInitOnly())
            {
                parts.Add("init;");
            }
            else if (property.CanWrite 
                && (property.SetMethod?.IsPublic ?? false))
            {
                parts.Add("set;");
            }
            parts.Add("}");
            return string.Join(' ', parts);
        }

        private static bool IsInitOnly(this PropertyInfo propertyInfo)
        {
            MethodInfo? setMethod = propertyInfo.SetMethod;
            if (setMethod == null)
                return false;

            var isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);
            return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
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
