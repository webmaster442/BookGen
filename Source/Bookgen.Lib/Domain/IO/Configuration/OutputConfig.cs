using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public abstract class OutputConfig
{
    [Required]
    public ImageConfig Images { get; init; }
    
    [Required]
    public CssClasses CssClasses { get; init; }

    [Required]
    public bool PreRenderCode {  get; init; }

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
