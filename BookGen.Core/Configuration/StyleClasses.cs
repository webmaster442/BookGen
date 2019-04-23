//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public class StyleClasses: ConfigurationBase
    {
        private string _Heading1;
        private string _Heading2;
        private string _Heading3;
        private string _Image;
        private string _Table;
        private string _Blockquote;
        private string _Figure;
        private string _FigureCaption;
        private string _Link;
        private string _OrderedList;
        private string _UnorederedList;
        private string _ListItem;

        public string Heading1
        {
            get => _Heading1;
            set => SetValue(ref _Heading1, value);
        }

        public string Heading2
        {
            get => _Heading2;
            set => SetValue(ref _Heading2, value);
        }

        public string Heading3
        {
            get => _Heading3;
            set => SetValue(ref _Heading3, value);
        }

        public string Image
        {
            get => _Image;
            set => SetValue(ref _Image, value);
        }

        public string Table
        {
            get => _Table;
            set => SetValue(ref _Table, value);
        }

        public string Blockquote
        {
            get => _Blockquote;
            set => SetValue(ref _Blockquote, value);
        }

        public string Figure
        {
            get => _Figure;
            set => SetValue(ref _Figure, value);
        }

        public string FigureCaption
        {
            get => _FigureCaption;
            set => SetValue(ref _FigureCaption, value);
        }

        public string Link
        {
            get => _Link;
            set => SetValue(ref _Link, value);
        }

        public string OrderedList
        {
            get => _OrderedList;
            set => SetValue(ref _OrderedList, value);
        }

        public string UnorederedList
        {
            get => _UnorederedList;
            set => SetValue(ref _UnorederedList, value);
        }

        public string ListItem
        {
            get => _ListItem;
            set => SetValue(ref _ListItem, value);
        }

        public StyleClasses()
        {
            _Heading1 = string.Empty;
            _Heading2 = string.Empty;
            _Heading3 = string.Empty;
            _Image = string.Empty;
            _Table = string.Empty;
            _Blockquote = string.Empty;
            _Figure = string.Empty;
            _FigureCaption = string.Empty;
            _Link = string.Empty;
            _OrderedList = string.Empty;
            _UnorederedList = string.Empty;
            _ListItem = string.Empty;
        }
    }
}
