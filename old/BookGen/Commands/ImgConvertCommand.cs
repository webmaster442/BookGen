//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("imgconvert")]
internal class ImgConvertCommand : Command<ImgConvertArguments>
{
    private readonly ILogger _log;

    public ImgConvertCommand(ILogger log)
    {
        _log = log;
    }

    public override int Execute(ImgConvertArguments arguments, string[] context)
    {

        if (arguments.Input.IsDirectory)
        {
            IEnumerable<Interfaces.FsPath>? files =
                arguments.Input.GetAllFiles(false).Where(ImageUtils.IsImage);

            Parallel.ForEach(files, file =>
            {
                Interfaces.FsPath? output = arguments.Output.Combine(file.Filename);

                ImageUtils.ConvertImageFile(_log,
                                            file,
                                            output,
                                            arguments.Quality,
                                            arguments.Width,
                                            arguments.Height,
                                            arguments.Format);
            });

            return Constants.Succes;
        }

        return ImageUtils.ConvertImageFile(_log,
                                           arguments.Input,
                                           arguments.Output,
                                           arguments.Quality,
                                           arguments.Width,
                                           arguments.Height)
            ? Constants.Succes
            : Constants.GeneralError;
    }
}
