//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public class BuildConfig
    {
        public string OutPutDirectory { get; set; }
        public string TemplateFile { get; set; }
        public List<Asset> TemplateAssets { get; set; }
        public StyleClasses StyleClasses { get; set; }
        public TemplateOptions TemplateOptions { get; set; }

        public static BuildConfig CreateDefault()
        {
            return new BuildConfig
            {
                OutPutDirectory = "Path to output directory",
                TemplateFile = "",
                TemplateAssets = new List<Asset>
                {
                    new Asset
                    {
                        Source = "",
                        Target = ""
                    }
                },
                TemplateOptions = TemplateOptions.CreateDefaultOptions(),
                StyleClasses = new StyleClasses()
            };
        }
    }
}
