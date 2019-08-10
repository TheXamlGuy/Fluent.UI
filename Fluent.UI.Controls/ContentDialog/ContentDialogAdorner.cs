using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    internal class ContentDialogAdorner : Adorner
    {
        private readonly AdornerLayer _adornerLayer;
        private UIElement _adorningElement;

        public ContentDialogAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);
        }

        public void Add(UIElement control)
        {
            _adorningElement = control;

            AddLogicalChild(control);
            AddVisualChild(control);

            Focusable = true;
        }

        public void Remove(UIElement control)
        {
            _adorningElement = null;

            RemoveLogicalChild(control);
            RemoveVisualChild(control);
        }

        public UIElement Fo()
        {
            return GetVisualChild(0) as UIElement;
        }

        protected override int VisualChildrenCount => _adorningElement == null ? 0 : 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _adorningElement != null)
            {
                return _adorningElement;
            }
            return base.GetVisualChild(index);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_adorningElement != null)
            {
                Point adorningPoint = new Point(0, 0);
                _adorningElement.Arrange(new Rect(adorningPoint, AdornedElement.RenderSize));
            }
            return finalSize;
        }
    }
}
