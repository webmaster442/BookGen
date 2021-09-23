//-----------------------------------------------------------------------
// <copyright file="MemberName.cs" company="Junle Li">
//     Copyright (c) Junle Li. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsxmd.Units
{
    /// <summary>
    /// Member name.
    /// </summary>
    internal class MemberName
    {
        private readonly string _name;
        private readonly char _type;
        private readonly IEnumerable<string> _paramNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberName"/> class.
        /// </summary>
        /// <param name="name">The raw member name. For example, <c>T:Vsxmd.Units.MemberName</c>.</param>
        /// <param name="paramNames">The parameter names. It is only used when member kind is <see cref="MemberKind.Constructor"/> or <see cref="MemberKind.Method"/>.</param>
        internal MemberName(string name, IEnumerable<string> paramNames)
        {
            _name = name;
            _type = name.First();
            _paramNames = paramNames;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberName"/> class.
        /// </summary>
        /// <param name="name">The raw member name. For example, <c>T:Vsxmd.Units.MemberName</c>.</param>
        internal MemberName(string name)
            : this(name, Enumerable.Empty<string>())
        {
        }

        /// <summary>
        /// Gets the member kind, one of <see cref="MemberKind"/>.
        /// </summary>
        /// <value>The member kind.</value>
        internal MemberKind Kind
        {
            get
            {
                return _type switch
                {
                    'T' => MemberKind.Type,
                    'F' => MemberKind.Constants,
                    'P' => MemberKind.Property,
                    'M' when _name.Contains(".#ctor", StringComparison.InvariantCulture) => MemberKind.Constructor,
                    'M' when !_name.Contains(".#ctor", StringComparison.InvariantCulture) => MemberKind.Method,
                    _ => MemberKind.NotSupported
                };
            }
        }

        /// <summary>
        /// Gets the link pointing to this member unit.
        /// </summary>
        /// <value>The link pointing to this member unit.</value>
        internal string Link
        {
            get
            {
                return Kind switch
                {
                    MemberKind.Type or MemberKind.Constants or MemberKind.Property => $"[{FriendlyName.Escape()}](#{Href} '{StrippedName}')",
                    MemberKind.Constructor or MemberKind.Method => $"[{FriendlyName.Escape()}({_paramNames.Join(",")})](#{Href} '{StrippedName}')",
                    _ => string.Empty
                };
            }
        }

        /// <summary>
        /// Gets the caption representation for this member name.
        /// </summary>
        /// <value>The caption.</value>
        /// <example>
        /// <para>For <see cref="MemberKind.Type"/>, show as <c>## Vsxmd.Units.MemberName [#](#here) [^](#contents)</c>.</para>
        /// <para>For other kinds, show as <c>### Vsxmd.Units.MemberName.Caption [#](#here) [^](#contents)</c>.</para>
        /// </example>
        internal string Caption
        {
            get
            {
                return Kind switch
                {
                    MemberKind.Type => $"{Href.ToAnchor()}## {FriendlyName.Escape()} `{Kind.ToLowerString()}`",
                    MemberKind.Constants or MemberKind.Property => $"{Href.ToAnchor()}### {FriendlyName.Escape()} `{Kind.ToLowerString()}`",
                    MemberKind.Constructor or MemberKind.Method => $"{Href.ToAnchor()}### {FriendlyName.Escape()}({_paramNames.Join(",")}) `{Kind.ToLowerString()}`",
                    _ => string.Empty
                };
            }
        }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        /// <value>The type name.</value>
        /// <example><c>Vsxmd.Program</c>, <c>Vsxmd.Units.TypeUnit</c>.</example>
        internal string TypeName =>
            $"{Namespace}.{TypeShortName}";

        /// <summary>
        /// Gets the namespace name.
        /// </summary>
        /// <value>The namespace name.</value>
        /// <example><c>System</c>, <c>Vsxmd</c>, <c>Vsxmd.Units</c>.</example>
        internal string Namespace
        {
            get
            {
                return Kind switch
                {
                    MemberKind.Type => NameSegments.TakeAllButLast(1).Join("."),
                    MemberKind.Constants or MemberKind.Property or MemberKind.Constructor or MemberKind.Method => NameSegments.TakeAllButLast(2).Join("."),
                    _ => string.Empty
                };
            }
        }

        internal string TypeShortName
        {
            get
            {
                return Kind switch
                {
                    MemberKind.Type => NameSegments.Last(),
                    MemberKind.Constants or MemberKind.Property or MemberKind.Constructor or MemberKind.Method => NameSegments.NthLast(2),
                    _ => string.Empty
                };
            }
        }

        private string Href => _name
            .Replace('.', '-')
            .Replace(':', '-')
            .Replace('(', '-')
            .Replace(')', '-');

        private string StrippedName =>
            _name.Substring(2);

        internal string LongName =>
            StrippedName.Split('(').First();

        private string MsdnName =>
            LongName.Split('{').First();

        private IEnumerable<string> NameSegments =>
            LongName.Split('.');

        private string FriendlyName
        {
            get
            {
                return Kind switch
                {
                    MemberKind.Type => TypeShortName,
                    MemberKind.Constants or MemberKind.Property or MemberKind.Constructor or MemberKind.Method => NameSegments.Last(),
                    _ => string.Empty
                };
            }
        }

        /// <summary>
        /// Gets the method parameter type names from the member name.
        /// </summary>
        /// <returns>The method parameter type name list.</returns>
        /// <example>
        /// It will prepend the type kind character (<c>T:</c>) to the type string.
        /// <para>For <c>(System.String,System.Int32)</c>, returns <c>["T:System.String","T:System.Int32"]</c>.</para>
        /// It also handle generic type.
        /// <para>For <c>(System.Collections.Generic.IEnumerable{System.String})</c>, returns <c>["T:System.Collections.Generic.IEnumerable{System.String}"]</c>.</para>
        /// </example>
        internal IEnumerable<string> GetParamTypes()
        {
            var paramString = _name.Split('(').Last().Trim(')');

            var delta = 0;
            var list = new List<StringBuilder>()
            {
                new StringBuilder("T:"),
            };

            foreach (var character in paramString)
            {
                if (character == '{')
                {
                    delta++;
                }
                else if (character == '}')
                {
                    delta--;
                }
                else if (character == ',' && delta == 0)
                {
                    list.Add(new StringBuilder("T:"));
                }

                if (character != ',' || delta != 0)
                {
                    list.Last().Append(character);
                }
            }

            return list.Select(x => x.ToString());
        }

        /// <summary>
        /// Convert the member name to Markdown reference link.
        /// <para>If then name is under <c>System</c> namespace, the link points to MSDN.</para>
        /// <para>Otherwise, the link points to this page anchor.</para>
        /// </summary>
        /// <param name="useShortName">Indicate if use short type name.</param>
        /// <returns>The generated Markdown reference link.</returns>
        internal string ToReferenceLink(bool useShortName)
        {
            if ($"{Namespace}.".StartsWith("System.", StringComparison.Ordinal))
            {
                return $"[{GetReferenceName(useShortName).Escape()}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:{MsdnName} '{StrippedName}')";
            }
            else
            {
                return $"[{GetReferenceName(useShortName).Escape()}](#{Href} '{StrippedName}')";
            }
        }

        private string GetReferenceName(bool useShortName)
        {
            if (!useShortName)
            {
                return LongName;
            }
            return Kind switch
            {
                MemberKind.Type => TypeShortName,
                MemberKind.Constants or MemberKind.Property or MemberKind.Method => FriendlyName,
                MemberKind.Constructor => $"{TypeShortName}.{FriendlyName}",
                _ => string.Empty
            };
        }
    }
}
