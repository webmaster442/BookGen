//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;

namespace BookGen.AssemblyDocumenter.Internals
{
    internal static class Helpers
    {
        public static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var nameandArgs = type.Name.Split('`');
            var genericPars =string.Join(", ", type.GetGenericArguments().Select(x => x.Name));

            return $"{nameandArgs[0]}<{genericPars}>";

        }
    }
}
