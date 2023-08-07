//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

using BookGen.Native.Implementations;

namespace BookGen.Native;
public static class NativeFactory
{
    public static IClipboard GetPlatformClipboard()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? new WindowsClipboard()
            : throw new PlatformNotSupportedException();
    }

    public static IProcessExtensions GetPlatformProcessExtensions()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new WindowsProcessExtensions()
            : new DumyProcessExtensions();
    }
}
