//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.VsTasks
{
    public class VsTaskRoot
    {
#pragma warning disable IDE1006 // Naming Styles
        public string? version { get; set; }
        public List<Task>? tasks { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
