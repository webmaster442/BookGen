//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Gui
{
    internal class Binder
    {
        private readonly object _model;
        private readonly Regex _propertyRegex;
        private readonly Type _modelType;

        public Binder(object model)
        {
            _model = model;
            _modelType = model.GetType();
            _propertyRegex = new Regex("\\{[a-zA-Z0-9]+\\}", RegexOptions.Compiled); 
        }

        public Action? InvokeCommand(string action)
        {
            if (!_propertyRegex.IsMatch(action))
                return null;

            var actionName = action.Replace("{", "").Replace("}", "");
            var prop = _modelType.GetProperty(actionName);
            DelegateCommand? cmd = prop?.GetValue(_model) as DelegateCommand;
            return cmd?.Action;
        }

        public bool IsBindableText(string text)
        {
            return _propertyRegex.IsMatch(text);
        }

        public string GetBoundString(string text)
        {
            StringBuilder buffer = new StringBuilder(text);
            foreach (Match? match in _propertyRegex.Matches(text))
            {
                if (match != null)
                    buffer.Replace(match.Value, GetPropertyValue(match.Value));
            }
            return buffer.ToString();
        }

        private string GetPropertyValue(string propertyName)
        {
            var prop = propertyName.Replace("{", "").Replace("}", "");
            if (_propertyRegex.IsMatch(prop))
                return string.Empty;

            var reflected = _modelType.GetProperty(prop);

            return reflected?.GetValue(_model)?.ToString() ?? "";

        }
    }
}
