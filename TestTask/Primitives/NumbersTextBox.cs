using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TestTask.Common;
using TestTask.ExtensionMethods;
using TestTask.Services;

namespace TestTask.Primitives
{
    public class NumbersTextBox : TextBox
    {
        public NumbersTextBox()
        {
            TextAlignment = TextAlignment.Right;

            PreviewTextInput += (s, e) =>
            {
                if (!char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                }

                if (AllowNegative && (e.Text[0] == '-' || e.Text[0] == '+') && SelectionStart == 0)
                {
                    e.Handled = false;
                }

                if (AllowDouble && SeparatorIndex(e.Text) != -1 && SeparatorIndex(Text) == -1)
                {
                    e.Handled = false;
                }

                if (SeparatorIndex(Text) != -1 && Text.Length - SeparatorIndex(Text) >= 3 && SelectionStart > SeparatorIndex(Text))
                {
                    e.Handled = true;
                }
            };

            TextChanged += (s, e) =>
            {
                if (_isInnerOperation)
                {
                    return;
                }

                var change = e.Changes.First(); // не дадим нажимать пробелы
                if (Text.Substring(change.Offset, change.AddedLength) == " ")
                {
                    _isInnerOperation = true;
                    Text = Text.Remove(change.Offset, change.AddedLength);
                    SelectionStart = change.Offset;
                    _isInnerOperation = false;
                    return;
                }
                
                var separator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                if (Text.Contains(".") && separator != "."
                    || Text.Contains(",") && separator != ",")
                {
                    ThreadHelper.RunInMainThread(() =>
                    {
                        var selStart = SelectionStart;
                        _isInnerOperation = true;
                        Text = Text.Replace(",", separator).Replace(".", separator);
                        _isInnerOperation = false;
                        SelectionStart = selStart;
                    });
                }

                _isInnerOperation = true;
                if (Text.Length == 1 && SeparatorIndex(Text) == 0)
                {
                    Text = "0.";
                    SelectionStart = Text.Length;
                }

                string text = "";
                Text.ForEach(c =>
                {
                    if (char.IsDigit(c)
                        || (c == '+' || c == '-' || c == '.' || c == ',') && !text.Contains(c))
                    {
                        text += c;
                    }
                });
                Value = double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var val)
                    ? val
                    : 0.0;

                _isInnerOperation = false;
            };

            LostFocus += (s, e) =>
            {
                if (Text.IsNullOrEmpty())
                {
                    _isInnerOperation = true;
                    Text = "0";
                    _isInnerOperation = false;
                }
            };
        }

        private int SeparatorIndex(string text)
        {
            var res = text.IndexOf(".");
            if (res == -1)
            {
                res = text.IndexOf(",");
            }

            return res;
        }

        private bool _isInnerOperation = false;

        public bool AllowNegative
        {
            get => (bool)GetValue(AllowNegativeProperty);
            set => SetValue(AllowNegativeProperty, value);
        }

        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(nameof(AllowNegative), typeof(bool), typeof(NumbersTextBox), new PropertyMetadata(true));


        public bool AllowDouble
        {
            get => (bool)GetValue(AllowDoubleProperty);
            set => SetValue(AllowDoubleProperty, value);
        }

        public static readonly DependencyProperty AllowDoubleProperty =
            DependencyProperty.Register(nameof(AllowDouble), typeof(bool), typeof(NumbersTextBox), new PropertyMetadata(true));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumbersTextBox),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    DefaultValue = 0.0,
                    PropertyChangedCallback = (s, e) => ((NumbersTextBox) s).OnValueChanged()
                });

        private void OnValueChanged()
        {
            if (_isInnerOperation)
            {
                return;
            }

            _isInnerOperation = true;
            Text = Math.Abs(Math.Round(Value) - Math.Round(Value, 2)) < 0.0001
                ? Value.ToString("N0")
                : Value.ToString("N2");
            _isInnerOperation = false;
        }
    }
}
