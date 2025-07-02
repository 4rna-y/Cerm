using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cerm.Ui.Watcher
{
    public class ConsoleWatcher : IConsoleWatcher
    {
        private int lastWidth;
        private int lastHeight;

        public void CancelDefaultKeyPress()
        {
            Console.TreatControlCAsInput = true;
            Console.CancelKeyPress += (o, e) =>
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC ||
                    e.SpecialKey == ConsoleSpecialKey.ControlBreak)
                    e.Cancel = true;
            };
        }

        public async Task WatchAsync(Action<int, int> onSizeChangedAction, CancellationToken token)
        {
            lastWidth = Console.WindowWidth;
            lastHeight = Console.WindowHeight;
            while (!token.IsCancellationRequested)
            {
                if (lastWidth != Console.WindowWidth || lastHeight != Console.WindowHeight)
                {
                    lastWidth = Console.WindowWidth;
                    lastHeight = Console.WindowHeight;
                    onSizeChangedAction(lastWidth, lastHeight);
                }

                await Task.Delay(100, token);
            }
        }
    }
}