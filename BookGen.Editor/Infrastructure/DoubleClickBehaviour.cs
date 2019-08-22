//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Editor.Infrastructure
{
    public class DoubleClickBehaviour
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickBehaviour), new UIPropertyMetadata(null, CommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(DoubleClickBehaviour), new UIPropertyMetadata(null));


        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control sender)
            {
                if (e.NewValue is ICommand cmd)
                    sender.MouseDoubleClick += Sender_MouseDoubleClick;
                else
                    sender.MouseDoubleClick -= Sender_MouseDoubleClick;
            }
        }

        private static void Sender_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var command = GetCommand(sender as DependencyObject);
            if (command != null)
            {
                command.Execute(GetCommandParameter(sender as DependencyObject));
            }
        }
    }
}

