//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Mvvm;
using BookGen.Gui.XmlEntities;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace BookGen.Gui
{
    internal sealed class Binder
    {
        private readonly WeakReference<ViewModelBase> _reference;
        private static readonly Regex _propertyRegex = new Regex("\\{[a-zA-Z0-9]+\\}", RegexOptions.Compiled);
        private readonly Type _modelType;

        private readonly List<(XView xmlEntity, View rendered, Type type)> _table;

        public Binder(ViewModelBase model)
        {
            _reference = new WeakReference<ViewModelBase>(model);
            _modelType = model.GetType();
            _table = new List<(XView xmlEntity, View rendered, Type type)>();
        }

        private string GetPropertyName(string bindingExpression)
        {
            return bindingExpression.Replace("{", "").Replace("}", "");
        }

        private string GetPropertyValue(string bindingExpression)
        {
            string? property = GetPropertyName(bindingExpression);
            if (_propertyRegex.IsMatch(property))
                return string.Empty;

            System.Reflection.PropertyInfo? reflected = _modelType.GetProperty(property);

            if (!_reference.TryGetTarget(out ViewModelBase? model))
                return string.Empty;

            return reflected?.GetValue(model)?.ToString() ?? "";

        }

        private T? GetPropertyValue<T>(string bindingExpression) where T : class
        {
            string? property = GetPropertyName(bindingExpression);
            if (_propertyRegex.IsMatch(property))
                return default;

            System.Reflection.PropertyInfo? reflected = _modelType.GetProperty(property);

            if (!_reference.TryGetTarget(out ViewModelBase? model))
                return default;

            return reflected?.GetValue(model) as T;

        }

        public string? BindCommand(string bindingExpression)
        {
            if (!_propertyRegex.IsMatch(bindingExpression))
                return null;

            string? actionName = GetPropertyName(bindingExpression);

            return actionName;
        }

        internal void TryInvokeCommand(string? command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            System.Reflection.PropertyInfo? prop = _modelType.GetProperty(command);

            if (prop == null || !_reference.TryGetTarget(out ViewModelBase? model))
                return;

            var cmd = prop?.GetValue(model) as DelegateCommand;

            if (cmd != null && cmd.SuspendsUI)
            {
                model.View?.SuspendUi();
                cmd.Action.Invoke();
                Console.WriteLine("Press a key to continue...");
                Console.ReadKey();
                model.View?.ResumeUi();
            }
            else
            {
                cmd?.Action.Invoke();
            }
        }

        public static bool IsBindable(string expression)
        {
            return _propertyRegex.IsMatch(expression);
        }

        public string GetBindedText(string text)
        {
            var buffer = new StringBuilder(text);
            foreach (Match? match in _propertyRegex.Matches(text))
            {
                if (match != null)
                    buffer.Replace(match.Value, GetPropertyValue(match.Value));
            }
            return buffer.ToString();
        }

        public bool GetBindedBool(string isChecked)
        {
            string? value = GetPropertyValue(isChecked);
            return Convert.ToBoolean(value);
        }

        internal IList GetBindedList(string itemSourceProperty)
        {
            IList? result = GetPropertyValue<IList>(itemSourceProperty);
            return result ?? new ArrayList();
        }

        public void Register(XView xmlEntity, View rendered, Type t)
        {
            _table.Add((xmlEntity, rendered, t));
        }

        public void UpdateFromModel()
        {
            foreach ((XView xmlEntity, View rendered, Type type) in _table)
            {
                switch (xmlEntity)
                {
                    case XCheckBox checkBox:
                        if (IsBindable(checkBox.IsChecked)
                            && rendered is CheckBox checkBoxRender
                            && type == typeof(bool))
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
            if (!_reference.TryGetTarget(out ViewModelBase? model))
                return;

            foreach ((XView xmlEntity, View rendered, Type type) in _table)
            {
                switch (xmlEntity)
                {
                    case XCheckBox checkBox:
                        if (IsBindable(checkBox.IsChecked)
                            && rendered is CheckBox checkBoxRender)
                        {
                            bool value = checkBoxRender.Checked;
                            string? property = GetPropertyName(checkBox.IsChecked);
                            _modelType.GetProperty(property)?.SetValue(model, value);

                        }
                        break;
                    case XListBox listBox:
                        if (IsBindable(listBox.SelectedIndex)
                            && rendered is ListView listView)
                        {
                            int value = listView.SelectedItem;
                            string? property = GetPropertyName(listBox.SelectedIndex);
                            _modelType.GetProperty(property)?.SetValue(model, value);
                        }
                        break;
                    case XRadioGroup radioGroup:
                        if (IsBindable(radioGroup.SelectedIndex)
                            && rendered is RadioGroup radio)
                        {
                            int value = radio.SelectedItem;
                            string? property = GetPropertyName(radioGroup.SelectedIndex);
                            _modelType.GetProperty(property)?.SetValue(model, value);
                        }
                        break;
                }
            }
        }
    }
}
