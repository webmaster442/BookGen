using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.IO.Configuration;

public abstract class OutputConfig
{
    [Required]
    public ImageConfig Images { get; init; }
    
    [Required]
    public CssClasses CssClasses { get; init; }

    public OutputConfig()
    {
        Images = new ImageConfig();
        CssClasses = new CssClasses();
    }
}
