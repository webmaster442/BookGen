//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Mvvm;
using BookGen.Gui.XmlEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal class Binder
    {
        private readonly object _model;
        private static readonly Regex _propertyRegex = new Regex("\\{[a-zA-Z0-9]+\\}", RegexOptions.Compiled);
        private readonly Type _modelType;

        private readonly List<(XView xmlEntity, View rendered, Type type)> _table;

        public Binder(object model)
        {
            _model = model;
            _modelType = model.GetType();
            _table = new List<(XView xmlEntity, View rendered, Type type)>();
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

        private T? GetPropertyValue<T>(string bindingExpression) where T: class
        {
            var property = GetPropertyName(bindingExpression);
            if (_propertyRegex.IsMatch(property))
                return default;

            var reflected = _modelType.GetProperty(property);

            return reflected?.GetValue(_model) as T;

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

        internal IList GetBindedList(string itemSourceProperty)
        {
            var result = GetPropertyValue<IList>(itemSourceProperty);
            return result ?? new ArrayList();
        }

        public void Register(XView xmlEntity, View rendered, Type t)
        {
            _table.Add((xmlEntity, rendered, t));
        }

        public void UpdateFromModel()
        {
            foreach (var (xmlEntity, rendered, type) in _table)
            {
                switch (xmlEntity)
                {
                    case XCheckBox checkBox:
                        if (IsBindable(checkBox.IsChecked)
                            && rendered is CheckBox checkBoxRender
                            && type== typeof(bool))
                        {
                            checkBoxRender.Checked = GetBindedBool(checkBox.IsChecked);
                        }
                        break;
                    case XTextBox textBox:
                        if (IsBindable(textBox.Text)
                            && rendered is TextView textView
                            && type == typeof(string))
                        {
                            textView.Text = GetBindedText(textBox.Text);
                        }
                        break;
                }
            }
        }

        public void UpdateToModel()
        {
            foreach (var (xmlEntity, rendered, type) in _table)
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
                    case XListBox listBox:
                        if (IsBindable(listBox.SelectedIndex)
                            && rendered is ListView listView)
                        {
                            var value = listView.SelectedItem;
                            var property = GetPropertyName(listBox.SelectedIndex);
                            _modelType.GetProperty(property)?.SetValue(_model, value);
                        }
                        break;
                    case XRadioGroup radioGroup:
                        if (IsBindable(radioGroup.SelectedIndex)
                            && rendered is RadioGroup radio)
                        {
                            var value = radio.SelectedItem;
                            var property = GetPropertyName(radioGroup.SelectedIndex);
                            _modelType.GetProperty(property)?.SetValue(_model, value);
                        }
                        break;
                }
            }
        }
    }
}
