//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Editor.ServiceContracts
{
    internal interface IBookGenServices
    {
        void BuildWebsite();
        void BuildEpub();
        void BuildTestWebsite();
        void BuildWordpress();
        void BuildPrint();
        void Clean();
        void Init();
    }
}
