using Cerm.Ui.Manager;
using Cerm.Config;
using Cerm.Ui.Screen;
using Cerm.Ui.Watcher;
using System.Threading.Tasks;
using System.Threading;

namespace Cerm
{
    public class CermApplication
    {
        private readonly IScreenManager screen;
        private readonly IConfigService config;
        private readonly IConsoleWatcher watcher;

        public CermApplication(
            IScreenManager screen,
            IConfigService config,
            IConsoleWatcher watcher
        )
        {
            this.screen = screen;
            this.config = config;
            this.watcher = watcher;
        }

        public void Run()
        {
            AppConfig cfg = config.Get();

            watcher.CancelDefaultKeyPress();

            CancellationTokenSource tokenSrc = new CancellationTokenSource();
            Task _ = watcher.WatchAsync(screen.OnResized, tokenSrc.Token);

            if (string.IsNullOrWhiteSpace(cfg.LastOpenedFolderPath))
            {
                screen.Push(new WelcomeScreen(screen));
            }
            else
            {

            }

            screen.Run();
        }
    }
}