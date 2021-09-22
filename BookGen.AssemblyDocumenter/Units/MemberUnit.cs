﻿//-----------------------------------------------------------------------
// <copyright file="MemberUnit.cs" company="Junle Li">
//     Copyright (c) Junle Li. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Vsxmd.Units
{
    /// <summary>
    /// Member unit.
    /// </summary>
    internal class MemberUnit : BaseUnit
    {
        private readonly MemberName _name;

        static MemberUnit()
        {
            Comparer = new MemberUnitComparer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberUnit"/> class.
        /// </summary>
        /// <param name="element">The member XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>member</c>.</exception>
        internal MemberUnit(XElement element)
            : base(element, "member")
        {
            _name = new MemberName(
                GetAttribute("name"),
                GetChildren("param").Select(x => x.Attribute("name")?.Value ?? string.Empty));
        }

        /// <summary>
        /// Gets the member unit comparer.
        /// </summary>
        /// <value>The member unit comparer.</value>
        internal static IComparer<MemberUnit> Comparer { get; }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        /// <value>The the type name.</value>
        /// <example><c>Vsxmd.Program</c>, <c>Vsxmd.Units.TypeUnit</c>.</example>
        internal string TypeName => _name.TypeName;

        /// <summary>
        /// Gets the member kind, one of <see cref="MemberKind"/>.
        /// </summary>
        /// <value>The member kind.</value>
        internal MemberKind Kind => _name.Kind;

        /// <summary>
        /// Gets the link pointing to this member unit.
        /// </summary>
        /// <value>The link pointing to this member unit.</value>
        internal string Link => _name.Link;

        private IEnumerable<string> InheritDoc
        {
            get
            {
                if (GetChild("inheritdoc") == null)
                {
                    return Enumerable.Empty<string>();
                }
                else
                {
                    return new[]
{
                        "##### Summary",
                        "*Inherit from parent.*",
                    };
                }
            }
        }

        private IEnumerable<string> Namespace
        {
            get
            {
                if (Kind != MemberKind.Type)
                {
                    return Enumerable.Empty<string>();
                }
                else
                {
                    return new[]
                    {
                        $"##### Namespace",
                        $"{_name.Namespace}",
                    };
                }
            }
        }

        private IEnumerable<string> Summary => SummaryUnit.ToMarkdown(GetChild("summary"));

        private IEnumerable<string> Returns => ReturnsUnit.ToMarkdown(GetChild("returns"));

        private IEnumerable<string> Params => ParamUnit.ToMarkdown(GetChildren("param"), _name.GetParamTypes(), Kind);

        private IEnumerable<string> Typeparams => TypeparamUnit.ToMarkdown(GetChildren("typeparam"));

        private IEnumerable<string> Exceptions => ExceptionUnit.ToMarkdown(GetChildren("exception"));

        private IEnumerable<string> Permissions => PermissionUnit.ToMarkdown(GetChildren("permission"));

        private IEnumerable<string> Example => ExampleUnit.ToMarkdown(GetChild("example"));

        private IEnumerable<string> Remarks => RemarksUnit.ToMarkdown(GetChild("remarks"));

        private IEnumerable<string> Seealsos => SeealsoUnit.ToMarkdown(GetChildren("seealso"));

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            var list = new List<string>();
            list.Add(_name.Caption);
            list.AddRange(Namespace);
            list.AddRange(Namespace);
            list.AddRange(InheritDoc);
            list.AddRange(Summary);
            list.AddRange(Returns);
            list.AddRange(Params);
            list.AddRange(Typeparams);
            list.AddRange(Exceptions);
            list.AddRange(Permissions);
            list.AddRange(Example);
            list.AddRange(Remarks);
            list.AddRange(Seealsos);
            return list;
        }

        /// <summary>
        /// Complement a type unit if the member unit <paramref name="group"/> does not have one.
        /// One member unit group has the same <see cref="TypeName"/>.
        /// </summary>
        /// <param name="group">The member unit group.</param>
        /// <returns>The complemented member unit group.</returns>
        internal static IEnumerable<MemberUnit> ComplementType(IEnumerable<MemberUnit> group)
        {
            if (group.Any(unit => unit.Kind == MemberKind.Type))
            {
                return group;
            }
            else
            {
                return group.Concat(new[] { Create(group.First().TypeName) });
            }
        }

        private static MemberUnit Create(string typeName)
        {
            return new MemberUnit(new XElement("member", new XAttribute("name", $"T:{typeName}")));
        }

        private class MemberUnitComparer : IComparer<MemberUnit>
        {
            public int Compare(MemberUnit? x, MemberUnit? y)
            {
                if (x != null && y != null)
                    return x._name.CompareTo(y._name);
                return -1;
            }
        }
    }
}
