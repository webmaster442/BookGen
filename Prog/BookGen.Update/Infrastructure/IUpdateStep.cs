﻿//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Update.Infrastructure;

internal interface IUpdateStep
{
    string StatusMessage { get; }
}
