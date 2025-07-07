using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cerm.Ui.Watcher
{
    public interface IConsoleWatcher
    {
        void CancelDefaultKeyPress();
        Task WatchAsync(Action<int, int> onSizeChangedAction, CancellationToken token);
    }
}