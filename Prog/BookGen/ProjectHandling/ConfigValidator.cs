﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;

namespace BookGen.ProjectHandling;

public sealed class ConfigValidator : Validator
{
    public ValidateFor ValidationLevel { get; set; }

    public enum ValidateFor
    {
        Web,
        Print,
        Epub,
        All,
    }

    private readonly Config _config;
    private readonly FsPath _workdir;

    public ConfigValidator(Config config, FsPath workdir)
    {
        ValidationLevel = ValidateFor.All;
        _config = config;
        _workdir = workdir;
    }

    private void ValidateBuildConfig(FsPath WorkDirectory, BuildConfig target)
    {
        if (target == null)
        {
            AddError(Properties.Resources.MissingSection);
            return;
        }

        if (!string.IsNullOrEmpty(target.TemplateFile)
            && !WorkDirectory.Combine(target.TemplateFile).IsExisting)
        {
            AddError(Properties.Resources.MissingTemplateFile, target.TemplateFile);
        }

        if (target.TemplateAssets == null)
        {
            AddError(Properties.Resources.MissingAssets);
        }
        else
        {
            foreach (Asset? asset in target.TemplateAssets)
            {
                FsPath? source = WorkDirectory.Combine(asset.Source);

                if (!source.IsExisting)
                    AddError(Properties.Resources.MissingAsset, source.ToString());
            }
        }

        if (target.StyleClasses == null)
            AddError(Properties.Resources.MissingStyleClasses);

        ValidateImageOptions(target.ImageOptions);
    }

    private void ValidateImageOptions(ImageOptions imageOptions)
    {
        if (imageOptions.InlineImageSizeLimit < 0)
            AddError(Properties.Resources.InvalidValueMustBePositive, nameof(imageOptions.InlineImageSizeLimit));

        if (imageOptions.MaxHeight < 0)
            AddError(Properties.Resources.InvalidValueMustBePositive, nameof(imageOptions.MaxHeight));

        if (imageOptions.MaxWidth < 0)
            AddError(Properties.Resources.InvalidValueMustBePositive, nameof(imageOptions.MaxWidth));

        if (imageOptions.ImageQuality < 0 || imageOptions.ImageQuality > 100)
            AddError(Properties.Resources.InvalidValueMustBeBetweenRange, nameof(imageOptions.MaxWidth), 0, 100);
    }

    public override void Validate()
    {
        if (Errors.Count > 0)
            Errors.Clear();

        switch (ValidationLevel)
        {
            case ValidateFor.Epub:
                ValidateBuildConfig(_workdir, _config.TargetEpub);
                break;
            case ValidateFor.Print:
                ValidateBuildConfig(_workdir, _config.TargetPrint);
                break;
            case ValidateFor.Web:
                ValidateBuildConfig(_workdir, _config.TargetWeb);
                break;
            case ValidateFor.All:
                ValidateBuildConfig(_workdir, _config.TargetEpub);
                ValidateBuildConfig(_workdir, _config.TargetPrint);
                ValidateBuildConfig(_workdir, _config.TargetWeb);
                break;
        }

        if (!string.IsNullOrEmpty(_config.ImageDir) && !_workdir.Combine(_config.ImageDir).IsExisting)
            AddError(Properties.Resources.MissingImageDir, _config.ImageDir);

        if (!_workdir.Combine(_config.TOCFile).IsExisting)
            AddError(Properties.Resources.MissingTocFile, _config.TOCFile);

        if (string.IsNullOrEmpty(_config.HostName))
        {
            AddError(Properties.Resources.MissingHostName);
        }
        else if (!_config.HostName.EndsWith("/"))
        {
            AddError(Properties.Resources.MissingTrailingSlash);
        }

        if (string.IsNullOrEmpty(_config.Index))
            AddError(Properties.Resources.MissingIndex);

        if (_config.Translations == null)
            AddError(Properties.Resources.MissingTranslations);
        if (_config.Metadata == null)
            AddError(Properties.Resources.MissingMetadata);
    }
}
