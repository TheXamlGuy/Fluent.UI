using System;
using System.Globalization;
using System.Windows;

namespace Fluent.UI.Controls
{
    public class EqualsStateTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty EqualToProperty =
            DependencyProperty.Register(nameof(EqualTo),
                typeof(object), typeof(EqualsStateTrigger),
                new PropertyMetadata(null, OnValuePropertyChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object), typeof(EqualsStateTrigger),
                new PropertyMetadata(null, OnValuePropertyChanged));

        private bool _isActive;

        public event EventHandler IsActiveChanged;

        public object EqualTo
        {
            get => GetValue(EqualToProperty);
            set => SetValue(EqualToProperty, value);
        }

        public bool IsActive
        {
            get => _isActive;
            private set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    SetActive(value);
                    IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        internal static bool AreValuesEqual(object value1, object value2, bool convertType)
        {
            if (value1 == value2)
            {
                return true;
            }

            if (value1 != null && value2 != null && convertType)
            {
                return ConvertTypeEquals(value1, value2) || ConvertTypeEquals(value2, value1);
            }

            return false;
        }

        private static object ConvertToEnum(Type enumType, object value)
        {
            try
            {
                return Enum.IsDefined(enumType, value) ? Enum.ToObject(enumType, value) : null;
            }
            catch
            {
                return null;
            }
        }

        private static bool ConvertTypeEquals(object value1, object value2)
        {
            if (value2 is Enum)
            {
                value1 = ConvertToEnum(value2.GetType(), value1);
            }
            else
            {
                value1 = Convert.ChangeType(value1, value2.GetType(), CultureInfo.InvariantCulture);
            }
            return value2.Equals(value1);
        }

        private static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var value = dependencyObject as EqualsStateTrigger;
            value?.UpdateTrigger();
        }

        private void UpdateTrigger()
        {
            IsActive = AreValuesEqual(Value, EqualTo, true);
        }
    }
}