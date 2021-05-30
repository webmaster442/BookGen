using BookGen.AssemblyDocument.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BookGen.AssemblyDocument
{
    public static class ReflectionExtensions
    {
        private static readonly Regex GenericStripper = new Regex(@"\`\d+", RegexOptions.Compiled);

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

        public static IEnumerable<Type> GetInheritanceChain(this Type type)
        {
            Type? cycle = type.BaseType;
            while (cycle != null)
            {
                yield return cycle;
                cycle = cycle.BaseType;
            }
        }

        public static string GetNormalizedTypeName(this Type type, bool fullName = true)
        {
            var selector = type.FullName ?? string.Empty;
            if (!string.IsNullOrEmpty(selector) 
                && Constants.KnownTypeNames.ContainsKey(selector))
            {
                return Constants.KnownTypeNames[selector];
            }

            if (!type.IsGenericType)
            {
                if (fullName)
                    return selector;
                else
                    return type.Name;
            }

            var name = GenericStripper.Replace(type.Name, "");

            var generalizedType = type.GetTypeInfo();
            if (!type.IsGenericTypeDefinition)
            {
                generalizedType = type.GetGenericTypeDefinition().GetTypeInfo();
            }

            var typeparams = string.Join(", ", generalizedType.GenericTypeParameters.Select(t => t.Name));

            if (fullName)
                return $"{type.Namespace}.{name}<{typeparams}>";

            return $"{name}<{typeparams}>";
        }

        public static string GetPropertyCode(this PropertyInfo property, bool isStatic = false)
        {
            List<string> parts = new List<string>();
            if (property.GetMethod?.IsPublic ?? false)
                parts.Add("public");
            else if (property.GetMethod?.IsFamilyOrAssembly ?? false)
                parts.Add("protected internal");
            else if (property.GetMethod?.IsAssembly ?? false)
                parts.Add("internal");
            else if (property.GetMethod?.IsFamily ?? false)
                parts.Add("protected");
            else
                parts.Add("private");

            if (isStatic)
                parts.Add("static");

            parts.Add(GetNormalizedTypeName(property.PropertyType) ?? "");
            parts.Add(property.Name);
            parts.Add("{");

            if (property.CanRead)
                parts.Add("get;");

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

        public static string GetMarkdownDocLinkFromType(this Type type)
        {
            string link = $"type://{GetNormalizedTypeName(type)}";

            if (type.FullName?.StartsWith("System.") ?? false)
            {
                if (type.IsGenericType)
                {
                    var generalizedType = type.GetTypeInfo();
                    if (!type.IsGenericTypeDefinition)
                    {
                        generalizedType = type.GetGenericTypeDefinition().GetTypeInfo();
                    }
                    var name = generalizedType.Name.Replace('`', '-');
                    link = $"https://docs.microsoft.com/en-us/dotnet/api/{type.Namespace}.{name}".ToLower();

                }
                else
                {
                    link = $"https://docs.microsoft.com/en-us/dotnet/api/{type.FullName}".ToLower();
                }
            }
            var linkname = type.GetNormalizedTypeName().Replace("<", "\\<").Replace(">", "\\>");
            return $"[{linkname}]({link})";
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
