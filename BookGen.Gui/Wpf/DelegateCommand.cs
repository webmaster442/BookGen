//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace BookGen.Gui.Wpf
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object?>? _canExecute;
        private readonly Action<object?> _execute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> execute)
                       : this(execute, null)
        {
        }

        public DelegateCommand(Action<object?> execute,
                       Predicate<object?>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
