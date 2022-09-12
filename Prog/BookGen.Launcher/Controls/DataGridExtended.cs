using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Launcher.Controls
{
    internal class DataGridExtended : DataGrid
    {
        public ICommand MouseDoubleClickCommand
        {
            get { return (ICommand)GetValue(MouseDoubleClickCommandProperty); }
            set { SetValue(MouseDoubleClickCommandProperty, value); }
        }

        public static readonly DependencyProperty MouseDoubleClickCommandProperty =
            DependencyProperty.Register("MouseDoubleClickCommand", typeof(ICommand), typeof(DataGridExtended), new PropertyMetadata(null));

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (MouseDoubleClickCommand != null
                && MouseDoubleClickCommand.CanExecute(SelectedItem))
            {
                MouseDoubleClickCommand.Execute(SelectedItem);
            }
        }
    }
}
