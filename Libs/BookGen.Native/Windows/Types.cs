//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace BookGen.Native.Windows;

[StructLayout(LayoutKind.Sequential)]
internal struct PROCESS_BASIC_INFORMATION
{
    public IntPtr Reserved1;
    public IntPtr PebBaseAddress;
    public IntPtr Reserved2_0;
    public IntPtr Reserved2_1;
    public IntPtr UniqueProcessId;
    public IntPtr Reserved3;
}

[StructLayout(LayoutKind.Sequential)]
internal struct UNICODE_STRING
{
    public short Length;
    public short MaximumLength;
    public IntPtr Buffer;
}

// for 32-bit process in a 64-bit OS only
[StructLayout(LayoutKind.Sequential)]
internal struct PROCESS_BASIC_INFORMATION_WOW64
{
    public long Reserved1;
    public long PebBaseAddress;
    public long Reserved2_0;
    public long Reserved2_1;
    public long UniqueProcessId;
    public long Reserved3;
}

// for 32-bit process in a 64-bit OS only
[StructLayout(LayoutKind.Sequential)]
internal struct UNICODE_STRING_WOW64
{
    public short Length;
    public short MaximumLength;
    public long Buffer;
}