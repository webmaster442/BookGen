using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpLoad.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal class CommentAttribute : Attribute
    {
        public string Value { get; }

        public CommentAttribute(string value)
        {
            Value = value;
        }
    }
}
