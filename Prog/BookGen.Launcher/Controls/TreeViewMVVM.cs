//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Launcher.Controls
{
    internal sealed class TreeViewMVVM : TreeView
    {
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register("ItemSelectedCommand", typeof(ICommand), typeof(TreeViewMVVM), new PropertyMetadata(null));


        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);
            if (ItemSelectedCommand != null
                && ItemSelectedCommand.CanExecute(SelectedItem))
            {
                ItemSelectedCommand.Execute(SelectedItem);
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue != null
                && newValue is IList list
                && list.Count > 0
                && ItemContainerGenerator.ContainerFromItem(list[0]) is TreeViewItem item)
            {
                item.IsSelected = true;
                item.IsExpanded = true;
            }
        }
    }
}
