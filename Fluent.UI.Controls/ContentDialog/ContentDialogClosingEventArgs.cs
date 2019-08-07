using System;

namespace Fluent.UI.Controls
{
    public class ContentDialogClosingEventArgs : EventArgs
    {
        public new static readonly ContentDialogClosingEventArgs Empty = new ContentDialogClosingEventArgs();

        private readonly object _eventDeferralLock = new object();

        private ContentDialogClosingDeferral _eventDeferral;

        public bool Cancel { get; set; }

        public ContentDialogResult Result { get; internal set; }

        public ContentDialogClosingDeferral GetDeferral()
        {
            lock (_eventDeferralLock)
            {
                return _eventDeferral ?? (_eventDeferral = new ContentDialogClosingDeferral());
            }
        }

        internal ContentDialogClosingDeferral GetCurrentDeferralAndReset()
        {
            lock (_eventDeferralLock)
            {
                var eventDeferral = _eventDeferral;
                _eventDeferral = null;
                return eventDeferral;
            }
        }
    }
}
