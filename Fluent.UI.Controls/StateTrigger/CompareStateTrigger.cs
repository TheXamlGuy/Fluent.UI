using System;
using System.Globalization;
using System.Windows;

namespace Fluent.UI.Controls
{
    public class CompareStateTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty CompareToProperty =
            DependencyProperty.Register(nameof(CompareTo),
                typeof(object), typeof(CompareStateTrigger),
                new PropertyMetadata(null, OnValuePropertyChanged));

        public static readonly DependencyProperty ComparisonProperty =
            DependencyProperty.Register("Comparison", typeof(Comparison),
                typeof(CompareStateTrigger), new PropertyMetadata(Comparison.Equal, OnValuePropertyChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(CompareStateTrigger),
            new PropertyMetadata(null, OnValuePropertyChanged));

        private bool _isActive;

        public event EventHandler IsActiveChanged;

        public object CompareTo
        {
            get => GetValue(CompareToProperty);
            set => SetValue(CompareToProperty, value);
        }

        public Comparison Comparison
        {
            get => (Comparison)GetValue(ComparisonProperty);
            set => SetValue(ComparisonProperty, value);
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

        internal Comparison CompareValues()
        {
            var v1 = Value;
            var v2 = CompareTo;

            if (v1 == v2)
            {
                if (Comparison == Comparison.Equal)
                {
                    return Comparison.Equal;
                }
            }

            if (v1 != null && v2 != null)
            {
                if (v1.GetType() != v2.GetType())
                {
                    if (v1 is Enum)
                    {
                        v2 = Enum.Parse(v1.GetType(), v2.ToString());
                    }
                    else if (v2 is Enum)
                    {
                        v1 = Enum.Parse(v2.GetType(), v1.ToString());
                    }
                    else if (v1 is IComparable)
                    {
                        v2 = Convert.ChangeType(v2, v1.GetType(), CultureInfo.InvariantCulture);
                    }
                    else if (v2 is IComparable)
                    {
                        v1 = Convert.ChangeType(v1, v2.GetType(), CultureInfo.InvariantCulture);
                    }
                }

                if (v1?.GetType() == v2?.GetType())
                {
                    if (v1 is IComparable comparable)
                    {
                        var result = comparable.CompareTo(v2);
                        if (result < 0)
                        {
                            return Comparison.LessThan;
                        }
                        return result == 0 ? Comparison.Equal : Comparison.GreaterThan;
                    }
                }
            }
            return Comparison.NotComparable;
        }

        private static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var value = dependencyObject as CompareStateTrigger;
            value?.UpdateTrigger();
        }

        private void UpdateTrigger()
        {
            IsActive = CompareValues() == Comparison;
        }
    }
}