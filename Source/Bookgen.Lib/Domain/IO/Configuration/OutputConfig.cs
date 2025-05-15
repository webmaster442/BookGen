using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public abstract class OutputConfig
{
    [Description("Image processing settings")]
    [Required]
    public ImageConfig Images { get; init; }

    [Description("Css classes to aply to various elements")]
    [Required]
    public CssClasses CssClasses { get; init; }

    [Description("Toggles pre-rendering of source code")]
    [Required]
    public bool PreRenderCode {  get; init; }

    [Description("Default template file name")]
    [NotNullOrWhiteSpace]
    public string DefaultTempate { get; init; }

    public OutputConfig()
    {
        Images = new ImageConfig();
        CssClasses = new CssClasses();
        PreRenderCode = false;
        DefaultTempate = string.Empty;
    }
}
