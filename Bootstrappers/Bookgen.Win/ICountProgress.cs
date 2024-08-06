using System;

namespace Bookgen.Win
{
    public interface ICountProgress : IProgress<int>
    {
        void SetMaximum(int max);
    }
}
