//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookGen.FormulaEdit.AppLogic;

internal static class Extensions
{
    public static void Update<T>(this BindingList<T> list, IEnumerable<T> items)
    {
        list.RaiseListChangedEvents = false;
        list.Clear();
        foreach (var item in items)
        {
            list.Add(item);
        }
        list.RaiseListChangedEvents = true;
        list.ResetBindings();
    }

    public static string GetExtension(this RenderFormat format)
    {
        return format switch
        {
            RenderFormat.Png => "png",
            RenderFormat.Svg => "svg",
            _ => throw new InvalidOperationException("Invalid render format"),
        };
    }
}
