using System;

namespace Fluent.UI.Controls
{
    public class SearchBoxQuerySubmittedEventArgs : EventArgs
    {
        internal SearchBoxQuerySubmittedEventArgs(string queryText)
        {
            QueryText = queryText;
        }

        public string QueryText { get; internal set; }
    }
}
