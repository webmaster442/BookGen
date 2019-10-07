//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using GalaSoft.MvvmLight.Messaging;

namespace BookGen.Editor.Models
{
    public class OpenFileMessage: MessageBase
    {
        public FsPath File { get; set; }
    }
}
