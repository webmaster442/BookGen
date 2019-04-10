//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Core
{
    public class FsPath : IEquatable<FsPath>
    {
        private string _path;

        public FsPath(params string[] pathParts)
        {
            _path = Path.Combine(pathParts);
        }

        public FsPath Combine(string part)
        {
            return new FsPath(_path, part);
        }

        public bool IsExisting
        {
            get
            {
                return Directory.Exists(_path) || File.Exists(_path);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FsPath);
        }

        public bool Equals(FsPath other)
        {
            return other != null &&
                   _path == other?._path;
        }

        public override int GetHashCode()
        {
            return 2090457805 + EqualityComparer<string>.Default.GetHashCode(_path);
        }

        public override string ToString()
        {
            return _path;
        }

        public static bool operator ==(FsPath path1, FsPath path2)
        {
            return path1?._path == path2?._path;
        }

        public static bool operator !=(FsPath path1, FsPath path2)
        {
            return !(path1 == path2);
        }
    }
}
