//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace BookGen.Native.Windows;

internal sealed partial class NtDll
{
    [LibraryImport("ntdll.dll")]
    internal static partial int NtQueryInformationProcess(IntPtr ProcessHandle,
                                                          int ProcessInformationClass,
                                                          ref PROCESS_BASIC_INFORMATION ProcessInformation,
                                                          int ProcessInformationLength,
                                                          IntPtr ReturnLength);

    // for 32-bit process in a 64-bit OS only
    [LibraryImport("ntdll.dll")]
    internal static partial int NtWow64QueryInformationProcess64(IntPtr ProcessHandle,
                                                                 int ProcessInformationClass,
                                                                 ref PROCESS_BASIC_INFORMATION_WOW64 ProcessInformation,
                                                                 int ProcessInformationLength,
                                                                 IntPtr ReturnLength);

    [LibraryImport("ntdll.dll")]
    internal static partial int NtWow64ReadVirtualMemory64(IntPtr hProcess,
                                                           long lpBaseAddress,
                                                           ref long lpBuffer,
                                                           long dwSize,
                                                           IntPtr lpNumberOfBytesRead);

    [LibraryImport("ntdll.dll")]
    internal static partial int NtWow64ReadVirtualMemory64(IntPtr hProcess,
                                                           long lpBaseAddress,
                                                           ref UNICODE_STRING_WOW64 lpBuffer,
                                                           long dwSize,
                                                           IntPtr lpNumberOfBytesRead);

    [LibraryImport("ntdll.dll")]
    internal static partial int NtWow64ReadVirtualMemory64(IntPtr hProcess,
                                                           long lpBaseAddress,
                                                           [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer,
                                                           long dwSize,
                                                           IntPtr lpNumberOfBytesRead);
}
