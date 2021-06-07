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
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            string selector = type.FullName ?? string.Empty;
            if (type.IsByRef)
            {
                selector = type?.FullName?[0..^1] ?? string.Empty;
            }

            if (!string.IsNullOrEmpty(selector) 
                && Constants.KnownTypeNames.ContainsKey(selector))
            {
                return Constants.KnownTypeNames[selector];
            }

            if (!type!.IsGenericType)
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

        public static IEnumerable<T> GetPublicProtected<T>(this IEnumerable<T> input)
            where T : MethodBase
        {
            return input.Where(m => m.IsPublic || m.IsFamily || m.IsFamilyOrAssembly);
        }

        public static IEnumerable<FieldInfo> GetPublicProtected(this IEnumerable<FieldInfo> input)
        {
            return input.Where(m => m.IsPublic || m.IsFamily || m.IsFamilyOrAssembly);
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
