//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;

namespace BookGen
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class AssemblyBuildDateAttribute: Attribute
    {
        public DateTime BuildDate { get; }

        public AssemblyBuildDateAttribute(string datestamp)
        {
            BuildDate = DateTime.ParseExact(datestamp,
                                            "yyyyMMddHHmmss", 
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.AssumeLocal);
        }
    }
}
