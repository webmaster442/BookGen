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
}
