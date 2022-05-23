//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Framework
{
    internal static class ClassDocumenter
    {
        private static readonly Dictionary<Type, string> CustomTypeDocs = new Dictionary<Type, string>(5);

        public static void DocumentType<T>(out string properties, out string typedocs) where T : class
        {
            CustomTypeDocs.Clear();
            properties = Document(typeof(T));
            typedocs = GenerateTypeDocs();
        }

        private static bool IsComplexType(Type propertyType)
        {
            bool baseClrType = propertyType == typeof(byte)
                         || propertyType == typeof(sbyte)
                         || propertyType == typeof(short)
                         || propertyType == typeof(ushort)
                         || propertyType == typeof(int)
                         || propertyType == typeof(uint)
                         || propertyType == typeof(long)
                         || propertyType == typeof(ulong)
                         || propertyType == typeof(decimal)
                         || propertyType == typeof(float)
                         || propertyType == typeof(double)
                         || propertyType == typeof(string)
                         || propertyType == typeof(bool);

            return !baseClrType;
        }

        private static string Document(Type inputType)
        {
            var result = new StringBuilder(1024);

            IEnumerable<PropertyInfo> properties = inputType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                string name = property.Name;
                Type type = property.PropertyType;

                DocAttribute? documentation = property.GetCustomAttributes<DocAttribute>().FirstOrDefault();

                if (documentation != null)
                {
                    Type typeToDocument = type;

                    if (type.IsGenericType)
                        typeToDocument = type.GetGenericArguments().First();

                    if (IsComplexType(typeToDocument)
                        && documentation.TypeAlias == null
                        && !CustomTypeDocs.ContainsKey(typeToDocument))
                    {
                        CustomTypeDocs.Add(typeToDocument, Document(typeToDocument));
                    }

                    result.AppendLine($"{name}");
                    result.AppendLine($"   Type: {GetTypeInfo(type, documentation.TypeAlias)}");
                    result.AppendFormat("   Can be left unchanged: {0}\r\n", documentation.IsOptional ? "yes" : "no");
                    result.AppendLine($"   {documentation.Description}\r\n");
                }
            }

            return result.ToString();
        }

        private static string GetTypeInfo(Type type, Type? typeAlias)
        {
            Type workType = type;
            if (typeAlias != null)
                workType = typeAlias;

            if (!workType.IsGenericType)
            {
                return workType.Name;
            }
            else
            {
                Type[] argumentTypes = workType.GetGenericArguments();
                string goodName = workType.Name.Replace($"`{argumentTypes.Length}", "<");
                return goodName + string.Join(", ", argumentTypes.Select(t => t.Name)) + ">";
            }
        }

        private static string GenerateTypeDocs()
        {
            var result = new StringBuilder(1024);

            foreach (var item in CustomTypeDocs)
            {
                result.AppendLine($"Properties of type: {item.Key.Name}");
                result.AppendLine("----------------------------------------");
                result.AppendLine(item.Value);
            }

            return result.ToString();
        }
    }
}
