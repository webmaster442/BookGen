using BookGen.AssemblyDocument.XmlDoc;
using System;
using System.Linq;

namespace BookGen.AssemblyDocument
{
    internal static class Selectors
    {
        public static string GetTypeSummary(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            if (member != null)
            {
                return member.Items?.OfType<Summary>()?.FirstOrDefault()?.JoinedText ?? string.Empty;
            }
            return string.Empty;
        }

        public static string GetTypeRemarks(this Doc documentation, Type t)
        {
            var member = Array.Find(documentation.Members.Items, m => m.Name == t.GetTypeSelectorName());
            if (member != null)
            {
                return member.Items?.OfType<Remarks>()?.FirstOrDefault()?.JoinedText ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
