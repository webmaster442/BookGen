//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public class Asset: ConfigurationBase
    {
        private string _source;
        private string _target;

        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        public string Target
        {
            get { return _target; }
            set { SetValue(ref _target, value); }
        }
    }
}
