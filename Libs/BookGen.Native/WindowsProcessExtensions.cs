//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using BookGen.Native.Windows;

namespace BookGen.Native;
internal sealed class WindowsProcessExtensions : IProcessExtensions
{
    public string GetWorkingDirectory(Process process)
    {
        //https://stackoverflow.com/questions/16110936/read-other-process-current-directory-in-c-sharp/23842609#23842609
        return GetProcessParametersString(process.Id, Environment.Is64BitOperatingSystem ? 0x38 : 0x24);
    }

    private const int PROCESS_QUERY_INFORMATION = 0x400;
    private const int PROCESS_VM_READ = 0x10;

    private static string GetProcessParametersString(int processId, int offset)
    {
        IntPtr handle = Kernel32.OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);
        if (handle == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        int processParametersOffset = Environment.Is64BitOperatingSystem ? 0x20 : 0x10;
        try
        {
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess) // are we running in WOW?
            {
                PROCESS_BASIC_INFORMATION_WOW64 pbi = new PROCESS_BASIC_INFORMATION_WOW64();
                int hr = NtDll.NtWow64QueryInformationProcess64(handle, 0, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                if (hr != 0)
                    throw new Win32Exception(hr);

                long pp = 0;
                hr = NtDll.NtWow64ReadVirtualMemory64(handle, pbi.PebBaseAddress + processParametersOffset, ref pp, Marshal.SizeOf(pp), IntPtr.Zero);
                if (hr != 0)
                    throw new Win32Exception(hr);

                UNICODE_STRING_WOW64 us = new UNICODE_STRING_WOW64();
                hr = NtDll.NtWow64ReadVirtualMemory64(handle, pp + offset, ref us, Marshal.SizeOf(us), IntPtr.Zero);
                if (hr != 0)
                    throw new Win32Exception(hr);

                if ((us.Buffer == 0) || (us.Length == 0))
                    return string.Empty;

                string s = new('\0', us.Length / 2);
                hr = NtDll.NtWow64ReadVirtualMemory64(handle, us.Buffer, s, us.Length, IntPtr.Zero);
                if (hr != 0)
                    throw new Win32Exception(hr);

                return s;
            }
            else // we are running with the same bitness as the OS, 32 or 64
            {
                PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
                int hr = NtDll.NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), IntPtr.Zero);
                if (hr != 0)
                    throw new Win32Exception(hr);

                IntPtr pp = new IntPtr();
                if (!Kernel32.ReadProcessMemory(handle, pbi.PebBaseAddress + processParametersOffset, ref pp, new IntPtr(Marshal.SizeOf(pp)), IntPtr.Zero))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                UNICODE_STRING us = new UNICODE_STRING();
                if (!Kernel32.ReadProcessMemory(handle, pp + offset, ref us, new IntPtr(Marshal.SizeOf(us)), IntPtr.Zero))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                if ((us.Buffer == IntPtr.Zero) || (us.Length == 0))
                    return string.Empty;

                string s = new('\0', us.Length / 2);
                if (!Kernel32.ReadProcessMemory(handle, us.Buffer, s, new IntPtr(us.Length), IntPtr.Zero))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                return s;
            }
        }
        finally
        {
            Kernel32.CloseHandle(handle);
        }
    }
}
