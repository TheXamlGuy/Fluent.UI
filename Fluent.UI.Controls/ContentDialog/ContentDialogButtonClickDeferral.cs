using System;
using System.Threading.Tasks;

namespace Fluent.UI.Controls
{
    public class ContentDialogButtonClickDeferral : IDisposable
    {
        private readonly TaskCompletionSource<object> _taskCompletionSource = new TaskCompletionSource<object>();

        internal ContentDialogButtonClickDeferral()
        {
        }

        public void Dispose()
        {
            Complete();
        }

        public void Complete()
        {
            _taskCompletionSource.TrySetResult(null);
        }

        internal async Task WaitForCompletion()
        {
            await _taskCompletionSource.Task;
        }
    }
}