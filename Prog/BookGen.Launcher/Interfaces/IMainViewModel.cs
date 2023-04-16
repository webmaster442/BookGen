//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.Interfaces;

internal interface IMainViewModel
{
    public void OpenPopupContent(INotifyPropertyChanged viewModel, string title);
    public void OpenContent(INotifyPropertyChanged viewModel);
    public void ClosePopup();
}
