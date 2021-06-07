using System.Collections.Generic;

namespace BookGen.AssemblyDocument
{
    internal static class Constants
    {
        public static readonly Dictionary<string, string> KnownTypeNames = new()
        {
            { "System.Byte", "byte" },
            { "System.SByte", "sbyte" },
            { "System.Int16", "short" },
            { "System.Int32", "int" },
            { "System.Int64", "long" },
            { "System.UInt16", "ushort" },
            { "System.UInt32", "uint" },
            { "System.UInt64", "ulong" },
            { "System.Single", "float" },
            { "System.Double", "double" },
            { "System.Decimal", "decimal" },
            { "System.String", "string" },
            { "System.Void", "void" },
        };
    }
}
