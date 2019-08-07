using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    [ContentProperty("Items")]
    public class NavigationHeader : Control
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items),
                typeof(ObservableCollection<object>), typeof(NavigationHeader),
                new PropertyMetadata(null));

        public static DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex),
                typeof(int), typeof(NavigationHeader),
                new PropertyMetadata(-1));

        public static DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                typeof(object), typeof(NavigationHeader),
                new PropertyMetadata(null));

        private NavigationHeaderList _pivotHeaderList;

        public NavigationHeader()
        {
            DefaultStyleKey = typeof(NavigationHeader);
            Items = new ObservableCollection<object>();
        }

        public event TypedEventHandler<NavigationHeader, NavigationHeaderItemInvokedEventArgs> ItemInvoked;

        public event TypedEventHandler<NavigationHeader, NavigationHeaderSelectionChangedEventArgs> SelectionChanged;

        public ObservableCollection<object> Items
        {
            get => (ObservableCollection<object>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public override void OnApplyTemplate()
        {
            PreparePivotHeaderList();
        }

        private void PreparePivotHeaderList()
        {
            _pivotHeaderList = GetTemplateChild("PivotHeaderList") as NavigationHeaderList;
            if (_pivotHeaderList != null)
            {
                _pivotHeaderList.MouseUp -= OnItemsHostClick;
                _pivotHeaderList.MouseUp += OnItemsHostClick;

                _pivotHeaderList.Loaded += OnPivotHeaderListLoaded;

            }
        }

        private void OnPivotHeaderListLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _pivotHeaderList.SelectionChanged -= OnSelectionChanged;
            _pivotHeaderList.SelectionChanged += OnSelectionChanged;

            var selectedIndexPropertyBinding = new Binding
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(SelectedIndexProperty)
            };

            BindingOperations.SetBinding(_pivotHeaderList, Selector.SelectedIndexProperty, selectedIndexPropertyBinding);
            PrepareItems();
        }

        private void RaiseSelectionChanged(object item)
        {
            SelectionChanged?.Invoke(this, new NavigationHeaderSelectionChangedEventArgs(item));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in args.NewItems)
                    {
                        _pivotHeaderList.Items.Add(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in args.OldItems)
                    {
                        _pivotHeaderList.Items.Remove(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnItemInvoke(object item)
        {
            ItemInvoked?.Invoke(this, new NavigationHeaderItemInvokedEventArgs(item));
        }

        private void OnItemsHostClick(object sender, MouseButtonEventArgs args)
        {
            var item = (sender as ItemsControl).GetListBoxItem(args.OriginalSource as DependencyObject);
            if (item != null)
            {
                OnItemInvoke(item);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var item = _pivotHeaderList.SelectedItem;
            if (item != null)
            {
                RaiseSelectionChanged(item);
            }
        }

        private void PrepareItems()
        {
            foreach (var item in Items)
            {
                _pivotHeaderList.Items.Add(item);
            }

            ((INotifyCollectionChanged)Items).CollectionChanged += OnCollectionChanged;
        }
    }
}
