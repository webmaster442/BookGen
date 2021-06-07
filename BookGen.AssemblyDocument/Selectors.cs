using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Linq;
using System.Reflection;

namespace BookGen.AssemblyDocument
{
    public static class Selectors
    {
        private static string GetSelectorName(this Type type)
        {
            return $"T:{type.FullName}";
        }

        private static string GetSelectorName(this PropertyInfo property)
        {
            return $"P:{property.ReflectedType?.FullName}.{property.Name}";
        }

        private static string GetSelectorName(this Type type, string field)
        {
            return $"F:{type.FullName}.{field}";
        }

        private static string GetSelectorName(this MethodInfo method)
        {
            return $"M:{method.ReflectedType?.FullName}.{method.Name}";
        }

        private static string GetSelectorName(this ConstructorInfo constructor)
        {
            var pars = constructor.GetParameters().Select(p => p.ParameterType.FullName);
            return $"M:{constructor.ReflectedType?.FullName}.#ctor({string.Join(',', pars)})";
        }

        private static string GetElementText<T>(Member? member) where T : Content
        {
            if (member != null)
            {
                return member.Items?.OfType<T>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }

        public static string GetTypeSummary(this Doc documentation, Type type)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == type.GetSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetConstructorSummary(this Doc documentation, ConstructorInfo constructorInfo)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == constructorInfo.GetSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetTypeRemarks(this Doc documentation, Type type)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == type.GetSelectorName());
            return GetElementText<Remarks>(member);
        }

        public static string GetPropertySummary(this Doc documentation, PropertyInfo property)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == property.GetSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetMethodSummary(this Doc documentation, MethodInfo methodInfo)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == methodInfo.GetSelectorName());
            return GetElementText<Summary>(member);
        }

        public static string GetEnumValueSummary(this Doc documentation, Type type, string enumItem)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == type.GetSelectorName(enumItem));
            if (member != null)
            {
                return member.Items?.OfType<Summary>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
