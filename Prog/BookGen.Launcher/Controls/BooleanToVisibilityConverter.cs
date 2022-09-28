//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.Controls
{
    internal sealed class BooleanToVisibilityConverter : ConverterBase<bool, Visibility>
    {
        protected override Visibility ConvertToTTo(bool tFrom, object parameter)
        {
            bool invert = false;
            if (parameter != null)
                invert = true;

            if (!invert && tFrom) return Visibility.Visible;
            else if (invert && !tFrom) return Visibility.Visible;
            else return Visibility.Collapsed;
        }
    }
}
