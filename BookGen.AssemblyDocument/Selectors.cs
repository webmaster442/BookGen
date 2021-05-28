using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Linq;

namespace BookGen.AssemblyDocument
{
    public static class Selectors
    {
        private static string GetTypeSelectorName(this Type type)
        {
            return $"T:{type.FullName}";
        }

        public static string GetTypeSummary(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            if (member != null)
            {
                return member.Items?.OfType<Summary>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }

        public static string GetTypeRemarks(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            if (member != null)
            {
                return member.Items?.OfType<Remarks>()?.FirstOrDefault()?.NormalizedText ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
