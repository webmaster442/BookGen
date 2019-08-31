//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using BookGen.Editor.ViewModel;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;

namespace BookGen.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                ConfigureApp();
            }
        }

        private void ConfigureApp()
        {
            App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            var dc = Locator.Resolve<MainViewModel>();
            dc.Editor = MdEditor;
            DataContext = dc;

            MdEditor.NHunspellServices = Locator.Resolve<INHunspellServices>();
            MdEditor.DialogService = Locator.Resolve<IDialogService>();
        }
    }
}
