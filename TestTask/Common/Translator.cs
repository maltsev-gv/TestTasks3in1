using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using TestTask.Models;

namespace TestTask.Common
{
    public class Translator : DependencyObject
    {
        public static void UpdateElems(MessageContainer messageContainer)
        {
            foreach (var keyValuePair in _translations)
            {
                var text = messageContainer != null
                    ? messageContainer[keyValuePair.Value.LangKey] ?? keyValuePair.Value.DefaultText
                    : keyValuePair.Value.DefaultText;
                keyValuePair.Value.TextPropertyInfo.SetValue(keyValuePair.Key, text);
            }
        }

        private static readonly Dictionary<DependencyObject, Translation> _translations = new Dictionary<DependencyObject, Translation>();

        public static DependencyProperty LangKeyProperty = DependencyProperty.RegisterAttached("LangKey",
            typeof(LangKeys), typeof(Translator), new PropertyMetadata(LangKeys.NotDefined, OnLangKeyPropertyChanged));

        private static void OnLangKeyPropertyChanged(DependencyObject elem, DependencyPropertyChangedEventArgs e)
        {
            if (!_translations.ContainsKey(elem))
            {
                var translation = new Translation();
                var allProps = elem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite).ToArray();
                translation.TextPropertyInfo = allProps.FirstOrDefault(p => p.Name == "Text")
                                               ?? allProps.FirstOrDefault(p => p.Name == "Header")
                                               ?? allProps.FirstOrDefault(p => p.Name == "Content");
                if (translation.TextPropertyInfo == null)
                {
                    throw new ArgumentException($"{nameof(Translator)}: element {elem.GetType().Name} doesn't content writable properties named Text, Header or Content");
                }

                translation.DefaultText = translation.TextPropertyInfo.GetValue(elem) as string;

                _translations[elem] = translation;
            }

            _translations[elem].LangKey = (LangKeys)e.NewValue;
        }

        public static void SetLangKey(DependencyObject elem, LangKeys value)
        {
            elem.SetValue(LangKeyProperty, value);
        }

        public static LangKeys GetLangKey(DependencyObject elem)
        {
            return (LangKeys)elem.GetValue(LangKeyProperty);
        }

        private class Translation
        {
            public PropertyInfo TextPropertyInfo { get; set; }
            public string DefaultText { get; set; }
            public LangKeys LangKey { get; set; }
        }
    }
}
