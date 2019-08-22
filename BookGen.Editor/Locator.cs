//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace BookGen.Editor
{
    public static class Locator
    {
        public static void Initialize()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ServiceContracts.IExceptionHandler, Services.ExceptionHandler>();
            SimpleIoc.Default.Register<ServiceContracts.IFileSystemServices, Services.FileSystemServices>();

            SimpleIoc.Default.Register<ViewModel.FileBrowserViewModel>();

        }

        public static T Resolve<T>()
        {
            return SimpleIoc.Default.GetInstance<T>();
        }
    }
}
