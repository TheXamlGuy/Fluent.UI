using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public class LoopingSelectorItem : ContentControl
    {
        internal LoopingSelector Owner;

        public LoopingSelectorItem()
        {
            DefaultStyleKey = typeof(LoopingSelectorItem);
            MouseUp += OnMouseUp;
            MouseDown += OnMouseDown;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        internal Rect RectPosition { get; set; }

        internal TranslateTransform GetTranslateTransform()
        {
            return (TranslateTransform)RenderTransform;
        }

        private void UpdateVisualStates()
        {
            var state = "Normal";
            if (IsMouseOver)
            {
                state = Mouse.LeftButton == MouseButtonState.Pressed ? "Pressed" : "PointerOver";
            }

            VisualStateManager.GoToState(this, state, true);
        }

        internal double GetVerticalPosition()
        {
            return RectPosition.Y + GetTranslateTransform().Y;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            UpdateVisualStates();
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            UpdateVisualStates();
        }

        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            UpdateVisualStates();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            Owner?.SetInternalSelection(this);
            UpdateVisualStates();
        }
    }
}
