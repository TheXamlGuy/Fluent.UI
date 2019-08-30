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
        }

        protected override int VisualChildrenCount => _adorningElement == null ? 0 : 1;

        public void Add(UIElement adorningElement)
        {
            _adornerLayer.Add(this);
            _adorningElement = adorningElement;

            AddLogicalChild(adorningElement);
            AddVisualChild(adorningElement);
        }

        public void Remove(UIElement adorningElement)
        {
            RemoveLogicalChild(adorningElement);
            RemoveVisualChild(adorningElement);

            _adornerLayer.Remove(this);
            _adorningElement = null;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _adorningElement != null) return _adorningElement;
            return base.GetVisualChild(index);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_adorningElement != null)
            {
                var adorningPoint = new Point(0, 0);
                _adorningElement.Arrange(new Rect(adorningPoint, AdornedElement.RenderSize));
            }

            return finalSize;
        }
    }
}