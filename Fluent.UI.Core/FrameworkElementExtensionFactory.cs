namespace Fluent.UI.Core
{
    internal sealed class FrameworkElementExtensionFactory
    {
        private static readonly object _lock = new object();
        private static FrameworkElementExtensionFactory _current = null;

        private FrameworkElementExtensionFactory()
        {
        }

        public static FrameworkElementExtensionFactory Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_current == null)
                    {
                        _current = new FrameworkElementExtensionFactory();
                    }
                    return _current;
                }
            }
        }
    }
}