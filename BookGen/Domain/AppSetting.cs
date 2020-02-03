//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;

namespace BookGen.Domain
{
    public class AppSetting: IAppSetting
    {
        public int NodeJsTimeout { get; set; }
        public string NodeJsPath { get; set; }

        public AppSetting()
        {
            NodeJsTimeout = 60;
            NodeJsPath = string.Empty;
        }
    }
}
