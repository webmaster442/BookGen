using System.Windows.Controls;

namespace BookGen.Launcher.Controls
{
    internal class PreviewControl : ContentControl
    {
        public PreviewControl()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public string FileUnderPreview
        {
            get { return (string)GetValue(FileUnderPreviewProperty); }
            set { SetValue(FileUnderPreviewProperty, value); }
        }

        public static readonly DependencyProperty FileUnderPreviewProperty =
            DependencyProperty.Register("FileUnderPreview", typeof(string), typeof(PreviewControl), new PropertyMetadata(null, OnChange));

        private static void OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PreviewControl preview)
                preview.LoadPreview();
        }

        private void LoadPreview()
        {
            string extension = Path.GetExtension(FileUnderPreview);
            if (extension == null)
                return;

            switch (extension)
            {
                case ".txt":
                case ".json":
                case ".js":
                case ".css":
                case ".cs":
                case ".gitignore":
                case ".editorconfig":
                case ".gitmodules":
                    OpenAsText(FileUnderPreview);
                    break;
                default:
                    DisplayError($"{extension} isn't supported by this preview");
                    break;
            }

        }

        private void DisplayError(string message)
        {
            var tb = new TextBlock
            {
                Style = (Style)FindResource("preview-error"),
                Text = message
            };
            Content = tb;
        }

        private void OpenAsText(string fileUnderPreview)
        {
            try
            {
                var tb = new TextBox
                {
                    Style = (Style)FindResource("preview-text"),
                    Text = File.ReadAllText(fileUnderPreview)
                };
                Content = tb;
            }
            catch (Exception)
            {
                DisplayError($"Error while loading: {fileUnderPreview}");
            }
        }
    }
}
