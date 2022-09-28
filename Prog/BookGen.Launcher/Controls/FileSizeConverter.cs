//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.Controls
{
    internal sealed class FileSizeConverter : ConverterBase<long, string>
    {
        private const double kiB = 1024.0;
        private const double MiB = 1048576.0;
        private const double GiB = 1073741824.0;
        private const double TiB = 1099511627776.0;
        private const double PiB = 1125899906842624.0;

        protected override string ConvertToTTo(long tFrom, object parameter)
        {
            if (tFrom > PiB)
                return Divide(tFrom, PiB, nameof(PiB));
            else if (tFrom > TiB)
                return Divide(tFrom, TiB, nameof(TiB));
            else if (tFrom > GiB)
                return Divide(tFrom, GiB, nameof(GiB));
            else if (tFrom > MiB)
                return Divide(tFrom, MiB, nameof(MiB));
            else if (tFrom > kiB)
                return Divide(tFrom, kiB, nameof(kiB));
            else
                return $"{tFrom} byte";
        }

        private string Divide(long value, double unit, string unitName)
        {
            double divided = Math.Round(value / unit, 3);
            return $"{divided} {unitName}";
        }
    }
}
