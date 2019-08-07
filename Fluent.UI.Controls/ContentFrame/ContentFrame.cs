using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Fluent.UI.Controls.ContentFrame
{
    public class ContentFrame : ContentControl
    {
        public static DependencyProperty BackStackDepthProperty =
            DependencyProperty.Register(nameof(BackStackDepth),
                typeof(int), typeof(ContentFrame),
                new PropertyMetadata(null));

        public static DependencyProperty BackStackProperty =
            DependencyProperty.Register(nameof(BackStack),
                typeof(IList<PageStackEntry>), typeof(ContentFrame),
                new PropertyMetadata(null));

        public static DependencyProperty CanGoBackProperty =
            DependencyProperty.Register(nameof(CanGoBack),
                typeof(bool), typeof(ContentFrame),
                new PropertyMetadata(null));

        public static DependencyProperty CurrentSourcePageTypeProperty =
            DependencyProperty.Register(nameof(CurrentSourcePageType),
                typeof(Type), typeof(ContentFrame),
                new PropertyMetadata(null));

        public static DependencyProperty SourcePageTypeProperty =
            DependencyProperty.Register(nameof(SourcePageType),
                typeof(Type), typeof(ContentFrame),
                new PropertyMetadata(null));

        private readonly object _lock = new object();

        private PageStackEntry _uncommitedPageStackEntry;

        public ContentFrame()
        {
            DefaultStyleKey = typeof(ContentFrame);
            BackStack = new ObservableCollection<PageStackEntry>();

            var notifyCollection = (INotifyCollectionChanged) BackStack;
            notifyCollection.CollectionChanged += OnCollectionChanged;
        }

        public event NavigatedEventHandler Navigated;

        public event NavigatingCancelEventHandler Navigating;

        public IList<PageStackEntry> BackStack
        {
            get => (IList<PageStackEntry>)GetValue(BackStackProperty);
            internal set => SetValue(BackStackProperty, value);
        }

        public int BackStackDepth
        {
            get => (int)GetValue(BackStackDepthProperty);
            internal set => SetValue(BackStackDepthProperty, value);
        }

        public bool CanGoBack
        {
            get => (bool)GetValue(CanGoBackProperty);
            internal set => SetValue(CanGoBackProperty, value);
        }

        public Type CurrentSourcePageType
        {
            get => (Type)GetValue(CurrentSourcePageTypeProperty);
            internal set => SetValue(CurrentSourcePageTypeProperty, value);
        }

        public Type SourcePageType
        {
            get => (Type)GetValue(SourcePageTypeProperty);
            internal set => SetValue(SourcePageTypeProperty, value);
        }

        public void GoBack()
        {
            if (!CanGoBack)
            {
                return;
            }

            var pageStackEntry = BackStack.LastOrDefault();
            if (pageStackEntry != null)
            {
                PrepareToNavigate(pageStackEntry.SourcePageType, pageStackEntry.Parameter, NavigationMode.Back);
            }
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            return PrepareToNavigate(sourcePageType, parameter, NavigationMode.New);
        }

        public bool Navigate(Type sourcePageType)
        {
            return PrepareToNavigate(sourcePageType, null, NavigationMode.New);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateBackState();
        }
        private bool PreparePage(Type type, object parameter, NavigationMode navigationMode)
        {
            lock (_lock)
            {
                var page = Activator.CreateInstance(type) as ContentPage;

                if (navigationMode == NavigationMode.New)
                {
                    if (_uncommitedPageStackEntry != null)
                    {
                        BackStack.Add(_uncommitedPageStackEntry);
                    }

                    _uncommitedPageStackEntry = new PageStackEntry(type, parameter);
                }

                if (navigationMode == NavigationMode.Back)
                {
                    var pageStackEntry = BackStack.LastOrDefault();
                    if (pageStackEntry != null)
                    {
                        BackStack.Remove(pageStackEntry);
                    }

                    _uncommitedPageStackEntry = pageStackEntry;
                }

                UpdateBackState();

                Content = page;

                if (Navigated != null)
                {
                    var args = new NavigationEventArgs
                    {
                        Content = page,
                        NavigationMode = navigationMode,
                        Parameter = parameter,
                        SourcePageType = SourcePageType
                    };

                    Navigated.Invoke(this, args);
                }

                // _isNavigating = false;

                return true;
            }
        }

        private bool PrepareToNavigate(Type type, object parameter, NavigationMode navigationMode)
        {
            //if (_isNavigating)
            //{
            //    return false;
            //}

            //_isNavigating = true;

            if (Navigating != null)
            {
                var args = new NavigatingCancelEventArgs
                {
                    NavigationMode = NavigationMode.Forward,
                    SourcePageType = type,
                    Parameter = parameter
                };

                foreach (var eventDelegate in Navigating.GetInvocationList())
                {
                    var handler = (NavigatingCancelEventHandler)eventDelegate;
                    handler(this, args);
                    if (args.Cancel)
                    {
                        return false;
                    }
                }
            }

            return PreparePage(type, parameter, navigationMode);
        }

        private void UpdateBackState()
        {
            CanGoBack = BackStack.Count > 0;
        }
    }
}
