using BookGen.Editor.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BookGen.Editor.ViewModel
{
    public class SpellCheckModel
    {
        public SpellCheckModel(INHunspellServices hunspellServices)
        {

            SpellCheckDictionaries = new ObservableCollection<string>();
        }

        public ObservableCollection<string> SpellCheckDictionaries { get; set; }
    }
}
