//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
using System;
using System.Runtime.Serialization;

namespace BookGen.Framework
{
    [Serializable]
    public class DependencyException : Exception
    {
        public DependencyException()
        {
        }

        public DependencyException(string? message) : base(message)
        {
        }

        public DependencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
