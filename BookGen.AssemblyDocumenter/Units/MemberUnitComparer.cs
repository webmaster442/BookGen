//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Vsxmd.Units
{
    internal partial class MemberUnit
    {
        internal class MemberUnitComparer : IComparer<MemberUnit>, IComparer<MemberName>
        {
            public int Compare(MemberUnit? x, MemberUnit? y)
            {
                if (x != null && y != null)
                    return Compare(x._name, y._name);
                return -1;
            }

            public int Compare(MemberName? x, MemberName? y)
            {
                if (x?.TypeShortName != y?.TypeShortName)
                {
                    return string.Compare(x?.TypeShortName, y?.TypeShortName, StringComparison.Ordinal);
                }
                else if (x?.Kind != y?.Kind)
                {
                    return x?.Kind.CompareTo(y?.Kind) ?? -1;
                }
                else
                {
                    return string.Compare(x?.LongName, y?.LongName, StringComparison.Ordinal);
                }
            }
        }
    }
}
