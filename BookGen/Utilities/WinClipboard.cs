//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on code from the TextCopy project - https://github.com/CopyText/TextCopy
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace BookGen.Utilities
{
    internal static class WinClipboard
    {
        private const uint cfUnicodeText = 13;

        public static void SetText(string text)
        {
            TryOpenClipboard();
            SetTextNative(text);
        }

        public static string? GetText()
        {
            if (!IsClipboardFormatAvailable(cfUnicodeText))
            {
                return null;
            }
            TryOpenClipboard();
            return GetTextNative();
        }

        private static void TryOpenClipboard()
        {
            var num = 10;
            while (true)
            {
                if (OpenClipboard(default))
                {
                    break;
                }
                if (--num == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                Thread.Sleep(100);
            }
        }

        private static void SetTextNative(string text)
        {
            EmptyClipboard();
            IntPtr hGlobal = default;
            try
            {
                var bytes = (text.Length + 1) * 2;
                hGlobal = Marshal.AllocHGlobal(bytes);

                if (hGlobal == default)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var target = GlobalLock(hGlobal);

                if (target == default)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                try
                {
                    Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                }
                finally
                {
                    GlobalUnlock(target);
                }

                if (SetClipboardData(cfUnicodeText, hGlobal) == default)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                hGlobal = default;
            }
            finally
            {
                if (hGlobal != default)
                    Marshal.FreeHGlobal(hGlobal);

                CloseClipboard();
            }
        }

        private static string? GetTextNative()
        {
            IntPtr handle = default;
            IntPtr pointer = default;
            try
            {
                handle = GetClipboardData(cfUnicodeText);
                if (handle == default)
                    return null;

                pointer = GlobalLock(handle);
                if (pointer == default)
                    return null;

                var size = GlobalSize(handle);
                var buff = new byte[size];
                Marshal.Copy(pointer, buff, 0, size);
                return Encoding.Unicode.GetString(buff).TrimEnd('\0');
            }
            finally
            {
                if (pointer != default)
                    GlobalUnlock(handle);

                CloseClipboard();
            }
        }

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GlobalSize(IntPtr hMem);

    }
}
