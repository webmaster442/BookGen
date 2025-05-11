//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shellprog.CommandCode.Organize;

public static class DefaultRulesProvider
{
    public static IEnumerable<OrganizeRule> Defaults
    {
        get
        {
            yield return new OrganizeRule
            {
                Patterns = ["*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.webp", "*.svg"],
                Destination = "Images"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.mp3", "*.flac", "*.wav", "*.ogg", "*.m4a", "*.wma"],
                Destination = "Audio"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.mp4", "*.mkv", "*.avi", "*.mov", "*.wmv", "*.flv", "*.webm"],
                Destination = "Video"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.pdf", "*.epub", "*.mobi", "*.azw", "*.djvu", "*.chm"],
                Destination = "Books"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.doc", "*.docx", "*.odt", "*.rtf"],
                Destination = "Documents"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.zip", "*.rar", "*.7z", "*.tar", "*.gz", "*.bz2"],
                Destination = "Archives"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.exe", "*.msi", "*.deb", "*.rpm", "*.pkg"],
                Destination = "Installers"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.txt", "*.log", "*.md", "*.markdown", "*.nfo"],
                Destination = "Texts"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.torrent"],
                Destination = "Torrents"
            };
            yield return new OrganizeRule
            {
                Patterns = ["*.iso", "*.img", "*.bin", "*.cue", "*.vhd", "*.vhdx"],
                Destination = "DiskImages"
            };
        }
    }
}
