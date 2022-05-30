//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

namespace BookGen
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class AssemblyBuildDateAttribute : Attribute
    {
        public DateTime BuildDateUtc { get; }

        public AssemblyBuildDateAttribute(string datestamp)
        {
            BuildDateUtc = DateTime.ParseExact(datestamp,
                                            "yyyyMMddHHmmss",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.AssumeLocal);
        }
    }
}
