﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BookGen.Core
{
    public sealed record FsPath : IComparable<FsPath>
    {
        private readonly string _path;

        private FsPath()
        {
            _path = string.Empty;
        }

        public FsPath(params string[] pathParts)
        {
            if (pathParts == null)
                throw new ArgumentNullException(nameof(pathParts));

            if (pathParts.Length < 1)
                throw new ArgumentException($"{nameof(pathParts)} must contain at least one element");

            var combined = Path.Combine(pathParts);

            if (combined.Contains("/") && combined.Contains("\\"))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    _path = combined.Replace("\\", "/");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _path = combined.Replace("/", "\\");
                }
                else
                {
                    _path = combined;
                }
            }
            else
            {
                _path = combined;
            }
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

        public string Extension
        {
            get
            {
                return Path.GetExtension(_path);
            }
        }

        public string Filename
        {
            get { return Path.GetFileName(_path); }
        }

        public override string ToString()
        {
            return _path;
        }

        public static FsPath Empty
        {
            get { return new FsPath(); }
        }

        public static bool IsEmptyPath(FsPath path)
        {
            return string.IsNullOrEmpty(path._path);
        }

        public int CompareTo(FsPath? other)
        {
            return _path.CompareTo(other?._path);
        }
    }
}
