//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public class Metadata : ConfigurationBase
    {
        private string _Author;
        private string _CoverImage;
        private string _Title;


        public string Author
        {
            get => _Author;
            set => SetValue(ref _Author, value);
        }

        public string CoverImage
        {
            get => _CoverImage;
            set => SetValue(ref _CoverImage, value);
        }

        public string Title
        {
            get => _Title;
            set => SetValue(ref _Title, value);
        }

        public static Metadata CreateDefault()
        {
            return new Metadata
            {
                Author = "Place author name here",
                CoverImage = "Place cover here",
                Title = "Book title"
            };
        }
    }
}
