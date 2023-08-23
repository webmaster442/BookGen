//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on code from the TextCopy project - https://github.com/CopyText/TextCopy
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using BookGen.Native.Windows;

namespace BookGen.Native.Implementations;

internal sealed class WindowsClipboard : IClipboard
{
    private const uint cfUnicodeText = 13;

    public void SetText(string text)
    {
        TryOpenClipboard();
        SetTextNative(text);
    }

    public string? GetText()
    {
        if (!User32.IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }
        TryOpenClipboard();
        return GetTextNative();
    }

    private static void TryOpenClipboard()
    {
        int num = 10;
        while (true)
        {
            if (User32.OpenClipboard(default))
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
        User32.EmptyClipboard();
        nint hGlobal = default;
        try
        {
            int bytes = (text.Length + 1) * 2;
            hGlobal = Marshal.AllocHGlobal(bytes);

            if (hGlobal == default)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            nint target = Kernel32.GlobalLock(hGlobal);

            if (target == default)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
            }
            finally
            {
                Kernel32.GlobalUnlock(target);
            }

            if (User32.SetClipboardData(cfUnicodeText, hGlobal) == default)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            hGlobal = default;
        }
        finally
        {
            if (hGlobal != default)
                Marshal.FreeHGlobal(hGlobal);

            User32.CloseClipboard();
        }
    }

    private static string? GetTextNative()
    {
        nint handle = default;
        nint pointer = default;
        try
        {
            handle = User32.GetClipboardData(cfUnicodeText);
            if (handle == default)
                return null;

            pointer = Kernel32.GlobalLock(handle);
            if (pointer == default)
                return null;

            int size = Kernel32.GlobalSize(handle);
            byte[]? buff = new byte[size];
            Marshal.Copy(pointer, buff, 0, size);
            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }
        finally
        {
            if (pointer != default)
                Kernel32.GlobalUnlock(handle);

            User32.CloseClipboard();
        }
    }
}
