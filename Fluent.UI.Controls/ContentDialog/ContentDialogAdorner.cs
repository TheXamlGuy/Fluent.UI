using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    internal class ContentDialogAdorner : Adorner
    {
        private readonly AdornerLayer _adornerLayer;
        public ContentDialog _adorningElement;

        public ContentDialogAdorner(UIElement adornedElement, ContentDialog adorningElement) : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adorningElement = adorningElement;
        }

        public void Show()
        {
            _adornerLayer.Add(this);

            AddLogicalChild(_adorningElement);
            AddVisualChild(_adorningElement);

            Focusable = true;
        }

        public void Close()
        {
            _adornerLayer.Remove(this);
            RemoveLogicalChild(_adorningElement);
            RemoveVisualChild(_adorningElement);
        }

        public ContentDialog Fo()
        {
            return GetVisualChild(0) as ContentDialog;
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
