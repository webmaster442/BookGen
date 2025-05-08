//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DocAttribute : Attribute
    {
        public bool IsOptional { get; set; }
        public string Description { get; set; }
        public Type? TypeAlias { get; set; }

        public DocAttribute(string description)
        {
            Description = description;
            IsOptional = false;
        }

        public DocAttribute(string description, bool optional)
        {
            Description = description;
            IsOptional = optional;
        }
    }
}
