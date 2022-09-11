namespace BookGen.Launcher.Interfaces
{
    internal interface IMainViewModel
    {
        public void OpenPopupContent(INotifyPropertyChanged viewModel);
        public void OpenContent(INotifyPropertyChanged viewModel);
        public void ClosePopup();
    }
}
