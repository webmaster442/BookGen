﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    internal abstract class DocumenterBase
    {
        public abstract void Document(MarkdownWriter targetDocument, Type type, XElement docSource);

        protected static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var nameandArgs = type.Name.Split('`');
            var genericPars = string.Join(", ", type.GetGenericArguments().Select(x => x.Name));

            return $"{nameandArgs[0]}<{genericPars}>";
        }
    }
}
