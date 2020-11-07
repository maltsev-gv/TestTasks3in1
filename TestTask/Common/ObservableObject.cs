using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TestTask.ExtensionMethods;

namespace TestTask.Common
{
    /// <summary>
    /// Реализует хранение значений свойств и быстрый доступ к ним из дочерних классов. Реализует INotifyPropertyChanged. Изменение свойства инициирует вызов PropertyChanged
    /// </summary>
    [Serializable]
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, object> _propDict = new Dictionary<string, object>();
        protected PropType GetVal<PropType>([CallerMemberName] string propName = null)
        {
            if (!_propDict.ContainsKey(propName))
            {
                var initValAttr = GetType().GetProperty(propName)?.GetCustomAttribute<InitialValueAttribute>();
                if (initValAttr != null)
                {
                    _propDict[propName] = initValAttr.InitialValue;
                }
                else
                {
                    _propDict[propName] = default(PropType);
                }
            }
            return (PropType)_propDict[propName];
        }

        protected bool SetVal(object newVal, Action actionAfter = null, [CallerMemberName] string propName = null)
        {
            if (_propDict.ContainsKey(propName) && _propDict[propName] != null && _propDict[propName].Equals(newVal))
            {
                return false;
            }
            _propDict[propName] = newVal;
            RaisePropertyChanged(propName);
            actionAfter?.Invoke();
            return true;
        }

        protected bool SetInitialVal(object initVal, [CallerMemberName] string propName = null)
        {
            if (_propDict.ContainsKey(propName))
            {
                return false;
            }
            _propDict[propName] = initVal;
            return true;
        }

        /// <summary>
        /// Копирует все public-свойства текущего объекта в destClass. 
        /// </summary>
        /// <param name="destClass">экземпляр класса того же типа, что и текущий, либо его наследник</param>
        /// <param name="silentPropertySet">если этот параметр выставлен (по умолчанию), то в destClass непустые сеттеры свойств по возможности не вызываются
        /// (отключается внутренняя логика класса и вызовы методов SetVal())</param>
        public void CopyAllPropertiesTo(ObservableObject destClass, bool silentPropertySet = true) 
        {
            var destType = destClass.GetType();
            var type = this.GetType();
            if (!type.IsAssignableFrom(destType))
            { 
                throw new InvalidCastException($"Object of type {destType.Name} is not assignable to type {type.Name}");
            }

            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            props.Where(p => p.CanWrite && (!silentPropertySet || !_propDict.ContainsKey(p.Name)))
                .ForEach(p => p.SetValue(destClass, p.GetValue(this)));
            if (silentPropertySet)
            {
                _propDict.ForEach(kvp => destClass._propDict[kvp.Key] = kvp.Value);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"{GetType().Name}: ");
            _propDict.ForEach(kvp => sb.Append($"{kvp.Key} : {(kvp.Value == null ? "null" : $"{kvp.Value}")}; "));
            return sb.ToString();
        }
    }

    /// <summary>
    /// Присваивает начальное значение свойству класса - наследника ObservableObject
    /// </summary>
    public class InitialValueAttribute : Attribute
    {
        public InitialValueAttribute(object value)
        {
            InitialValue = value;
        }

        public object InitialValue { get; }
    }
}
