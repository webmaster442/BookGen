//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Infrastructure.Tools;

internal interface IDownloadUi : IProgress<long>
{
    void BeginNew(string message, long maximum);
    void Error(string message);
}
