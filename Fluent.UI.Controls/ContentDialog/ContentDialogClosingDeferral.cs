using System;
using System.Threading.Tasks;

namespace Fluent.UI.Controls
{
    public class ContentDialogClosingDeferral : IDisposable
    {
        private readonly TaskCompletionSource<object> _taskCompletionSource = new TaskCompletionSource<object>();

        internal ContentDialogClosingDeferral()
        {
        }

        public void Complete() => _taskCompletionSource.TrySetResult(null);

        public void Dispose()
        {
            Complete();
        }

        internal async Task WaitForCompletion()
        {
            await _taskCompletionSource.Task;
        }
    }
}