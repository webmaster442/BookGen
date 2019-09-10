//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace BookGen.Editor
{
    public static class Locator
    {
        public static void Initialize()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IExceptionHandler, Services.ExceptionHandler>();
            SimpleIoc.Default.Register<IFileSystemServices, Services.FileSystemServices>();
            SimpleIoc.Default.Register<INHunspellServices, Services.NHuspellServices>();
            SimpleIoc.Default.Register<IDialogService, Services.DialogService>();
            SimpleIoc.Default.Register<IBookGenServices, Services.BookGenServices>();

            SimpleIoc.Default.Register<ViewModel.MainViewModel>(() =>
            {
                return new ViewModel.MainViewModel(Resolve<IFileSystemServices>(), Resolve<IExceptionHandler>());
            });
        }

        public static T Resolve<T>()
        {
            return SimpleIoc.Default.GetInstance<T>();
        }
    }
}
