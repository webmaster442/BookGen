//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BookGen.Lib.Rendering.Templates.Expressions;

internal enum TokenType
{
    None = 0,
    Integer = 1,
    Double = 2,
    Boolean = 4,
    String = 8,
    ArgumentDelimiter = 16,
    Variable = 32,
    Function = 64,
    OpenParen = 128,
    CloseParen = 256,
    EOF = int.MaxValue,
}
