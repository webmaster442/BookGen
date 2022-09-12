﻿using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Launcher.Controls
{
    internal class TreeViewMVVM : TreeView
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
    }
}
