using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class LoopingSelector : Selector
    {
        private RepeatButton _downButton;
        private LoopingSelectorPanel _panel;
        private RepeatButton _upButton;

        public LoopingSelector()
        {
            DefaultStyleKey = typeof(LoopingSelector);
            Loaded += OnLoaded;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            PreviewMouseWheel += OnMouseWheel;
        }

        public override void OnApplyTemplate()
        {
            _upButton = GetTemplateChild("UpButton") as RepeatButton;
            if (_upButton != null)
            {
                _upButton.Click -= OnUpButtonClick;
                _upButton.Click += OnUpButtonClick;
            }

            _downButton = GetTemplateChild("DownButton") as RepeatButton;
            if (_downButton != null)
            {
                _downButton.Click -= OnDownButtonClick;
                _downButton.Click += OnDownButtonClick;
            }
        }

        internal void SetInternalSelection(LoopingSelectorItem item)
        {
            var index = ItemContainerGenerator.IndexFromContainer(item);
            SetValue(SelectedIndexProperty, index);
        }

        internal void ScrollToSelectedItem(bool useTransitions)
        {
            if (!IsLoaded)
            {
                return;
            }

            if (_panel == null)
            {
                return;
            }

            if (SelectedIndex >= 0)
            {
                if (!(ItemContainerGenerator.ContainerFromIndex(SelectedIndex) is LoopingSelectorItem item))
                {
                    return;
                }

                _panel.ScrollToSelectedIndex(item, useTransitions);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new LoopingSelectorItem();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            ScrollToSelectedItem(true);
            base.OnSelectionChanged(e);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element is LoopingSelectorItem loopingSelectorItem)
            {
                loopingSelectorItem.Height = 44;
                loopingSelectorItem.UpdateLayout();
                loopingSelectorItem.Owner = this;
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        private void OnDownButtonClick(object sender, RoutedEventArgs args)
        {
            ScrollDown();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            if (_panel == null)
            {
                _panel = this.FindDescendant<LoopingSelectorPanel>();
            }

            ScrollToSelectedItem(false);
        }

        private void OnMouseEnter(object sender, MouseEventArgs args)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        private void OnMouseLeave(object sender, MouseEventArgs args)
        {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs args)
        {
            if (args.Delta > 0)
            {
                ScrollUp();
            }
            else
            {
                ScrollDown();
            }
        }

        private void OnUpButtonClick(object sender, RoutedEventArgs args)
        {
            ScrollUp();
        }

        private void ScrollDown()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (SelectedIndex == -1)
            {
                SetValue(SelectedIndexProperty, 0);
            }

            if (SelectedIndex == Items.Count - 1)
            {
                SetValue(SelectedIndexProperty, 0);
            }
            else
            {
                SetValue(SelectedIndexProperty, ++SelectedIndex);
            }
        }

        private void ScrollUp()
        {
            if (!IsLoaded)
            {
                return;
            }

            if (SelectedIndex == -1)
            {
                SetValue(SelectedIndexProperty, 0);
            }

            if (SelectedIndex == 0)
            {
                SetValue(SelectedIndexProperty, Items.Count - 1);
            }
            else
            {
                SetValue(SelectedIndexProperty, --SelectedIndex);
            }
        }
    }
}
