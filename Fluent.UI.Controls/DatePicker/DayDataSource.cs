using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Controls
{
    public class DayDataSource : DependencyObject
    {
        internal static DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date),
                typeof(DateTime), typeof(DayDataSource),
                new PropertyMetadata(DateTime.Now, OnDatePropertyChanged));

        internal static DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items),
                typeof(ObservableCollection<TimePickerItem>), typeof(DayDataSource),
                new PropertyMetadata(null));

        internal static DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                typeof(TimePickerItem), typeof(DayDataSource),
                new PropertyMetadata(null));

        private DateTime _previousDateTime;

        public DayDataSource()
        {
            Items = new ObservableCollection<TimePickerItem>();
        }

        internal DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        internal ObservableCollection<TimePickerItem> Items
        {
            get => (ObservableCollection<TimePickerItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        internal TimePickerItem SelectedItem
        {
            get => (TimePickerItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        internal void SetDefaultItem(DateTime dateTime)
        {
            var item = Items.FirstOrDefault(x => x.Value.Equals(dateTime.Day));
            if (item != null)
            {
                SetValue(SelectedItemProperty, item);
            }
        }

        private static void OnDatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var dayDataSource = dependencyObject as DayDataSource;
            dayDataSource?.OnDatePropertyChanged((DateTime)args.OldValue);
        }

        private void OnDatePropertyChanged(DateTime oldDateTime)
        {
            if (Date.Month == _previousDateTime.Month && Date.Year == _previousDateTime.Year)
            {
                return;
            }

            Items.Clear();

            var days = DateTime.DaysInMonth(Date.Year, Date.Month);
            for (var i = 0; i < days; i++)
            {
                Items.Add(new TimePickerItem
                {
                    Value = i + 1,
                    PrimaryText = (i + 1).ToString()
                });
            }

            SetValue(SelectedItemProperty, Items.FirstOrDefault());
            _previousDateTime = oldDateTime;
        }
    }
}