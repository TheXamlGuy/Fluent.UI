using System;

namespace Fluent.UI.Controls
{
    public class ContentDialogButtonClickEventArgs : EventArgs
    {
        internal new static readonly ContentDialogButtonClickDeferral Empty = new ContentDialogButtonClickDeferral();

        private readonly object _eventDeferralLock = new object();

        private ContentDialogButtonClickDeferral _eventDeferral;

        public bool Cancel { get; set; }

        public ContentDialogButtonClickDeferral GetDeferral()
        {
            lock (_eventDeferralLock)
            {
                return _eventDeferral ?? (_eventDeferral = new ContentDialogButtonClickDeferral());
            }
        }

        internal ContentDialogButtonClickDeferral GetCurrentDeferralAndReset()
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