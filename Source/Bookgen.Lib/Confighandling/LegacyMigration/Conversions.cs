//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Domain.IO.Legacy;

using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Bookgen.Lib.Confighandling.LegacyMigration;

internal static class Conversions
{
    public static CssClasses ToCssClasses(this StyleClasses styleClasses)
    {
        return new CssClasses
        {
            H1 = styleClasses.Heading1,
            H2 = styleClasses.Heading2,
            H3 = styleClasses.Heading3,
            Blockquote = styleClasses.Blockquote,
            Figure = styleClasses.Figure,
            FigureCaption = styleClasses.FigureCaption,
            Img = styleClasses.Image,
            Li = styleClasses.ListItem,
            Link = styleClasses.Link,
            Ol = styleClasses.OrderedList,
            Ul = styleClasses.UnorederedList,
            Table = styleClasses.Table,
        };
    }

    public static ImageConfig ToImageConfig(this ImageOptions imageOptions)
    {
        return new ImageConfig
        {
            ResizeAndRecodeImages = MapResizeOpion(imageOptions),
            ResizeHeight = imageOptions.MaxHeight,
            ResizeWith = imageOptions.MaxWidth,
            ImageQualityOnResize = imageOptions.ImageQuality,
            SvgRecode = imageOptions.SvgPassthru ? SvgRecodeOption.Passtrough : SvgRecodeOption.AsPng,
        };
    }

    private static ImgRecodeOption MapResizeOpion(ImageOptions imageOptions)
    {
        if (imageOptions.RecodeJpegToWebp || imageOptions.RecodePngToWebp || imageOptions.EnableResize)
            return ImgRecodeOption.AsWebp;
     
        if (imageOptions.EnableResize)
            return ImgRecodeOption.AsPng;

        return ImgRecodeOption.Passtrough;
    }
}
