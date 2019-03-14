//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace BookGen.Framework.UI
{
    internal class DelegateCommand<TArgs> : ICommand
    {
        public DelegateCommand(Action<TArgs> exDelegate)
        {
            _exDelegate = exDelegate;
        }

        public DelegateCommand(Action<TArgs> exDelegate, Predicate<TArgs> canDelegate)
        {
            _exDelegate = exDelegate;
            _canDelegate = canDelegate;
        }

        protected Action<TArgs> _exDelegate;
        protected Predicate<TArgs> _canDelegate;

        public bool CanExecute(TArgs parameter)
        {
            if (_canDelegate == null)
                return true;

            return _canDelegate(parameter);
        }

        public void Execute(TArgs parameter)
        {
            _exDelegate?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (parameter != null)
            {
                var parameterType = parameter.GetType();
                if (parameterType.FullName.Equals("MS.Internal.NamedObject"))
                    return false;
            }

            return CanExecute((TArgs)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((TArgs)parameter);
        }
    }

    internal static class DelegateCommand
    {
        public static ICommand CreateCommand(Action<object> action)
        {
            return new DelegateCommand<object>(action);
        }

        public static ICommand CreateCommand(Action<object> action, Predicate<object> predicate)
        {
            return new DelegateCommand<object>(action, predicate);
        }
    }
}
