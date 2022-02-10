﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace WpLoad.Infrastructure
{
    internal static class Extensions
    {
        const string XmlCommentPropertyPostfix = "XmlComment";

        public static bool TryGetArgument<T>(this IReadOnlyList<string> args, int index, [NotNullWhen(true)] out T? parsed) where T : IConvertible
        {
            if (index > args.Count || index < 0)
            {
                parsed = default;
                return false;
            }

            try
            {
                parsed = (T)Convert.ChangeType(args[index], typeof(T));
                return true;
            }
            catch (Exception)
            {
                parsed = default;
                return false;
            }
        }

    }
}
