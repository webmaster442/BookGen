//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Ui.Mvvm;
using BookGen.Ui.XmlEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace BookGen.Ui
{
    internal class Binder
    {
        private readonly object _model;
        private static readonly Regex _propertyRegex = new Regex("\\{[a-zA-Z0-9]+\\}", RegexOptions.Compiled);
        private readonly Type _modelType;

        private readonly List<(XView xmlEntity, View rendered)> _table;

        public Binder(object model)
        {
            _model = model;
            _modelType = model.GetType();
            _table = new List<(XView xmlEntity, View rendered)>();
        }

        public Action? BindCommand(string bindingExpression)
        {
            if (!_propertyRegex.IsMatch(bindingExpression))
                return null;

            var actionName = GetPropertyName(bindingExpression);
            var prop = _modelType.GetProperty(actionName);
            DelegateCommand? cmd = prop?.GetValue(_model) as DelegateCommand;
            return cmd?.Action;
        }

        private string GetPropertyName(string bindingExpression)
        {
            return bindingExpression.Replace("{", "").Replace("}", "");
        }

        private string GetPropertyValue(string bindingExpression)
        {
            var property = GetPropertyName(bindingExpression);
            if (_propertyRegex.IsMatch(property))
                return string.Empty;

            var reflected = _modelType.GetProperty(property);

            return reflected?.GetValue(_model)?.ToString() ?? "";

        }

        public static bool IsBindable(string expression)
        {
            return _propertyRegex.IsMatch(expression);
        }

        public string GetBindedText(string text)
        {
            StringBuilder buffer = new StringBuilder(text);
            foreach (Match? match in _propertyRegex.Matches(text))
            {
                if (match != null)
                    buffer.Replace(match.Value, GetPropertyValue(match.Value));
            }
            return buffer.ToString();
        }

        public bool GetBindedBool(string isChecked)
        {
            var value = GetPropertyValue(isChecked);
            return Convert.ToBoolean(value);
        }

        public void Register(XView xmlEntity, View rendered)
        {
            _table.Add((xmlEntity, rendered));
        }

        public void Update()
        {
            foreach (var (xmlEntity, rendered) in _table)
            {
                switch (xmlEntity)
                {
                    case XCheckBox checkBox:
                        if (IsBindable(checkBox.IsChecked)
                            && rendered is CheckBox checkBoxRender)
                        {
                            var value = checkBoxRender.Checked;
                            var property = GetPropertyName(checkBox.IsChecked);
                            _modelType.GetProperty(property)?.SetValue(_model, value);

                        }
                        break;
                }
            }
        }
    }
}
