//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Gui
{
    internal class Binder
    {
        private readonly object _model;

        public Binder(object model)
        {
            _model = model;
        }

        public Action InvokeCommand(string action)
        {
            var prop = _model.GetType().GetProperty(action);
            DelegateCommand cmd = prop?.GetValue(_model) as DelegateCommand;
            return cmd?.Action;
        }
    }
}
