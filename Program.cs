using System;
using Microsoft.Extensions.DependencyInjection;
using Cerm.Ui.Manager;
using Cerm.Config;
using Cerm.Ui.Watcher;

namespace Cerm
{
    public class Program
    {
        private IServiceProvider provider = null;
        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            provider = new ServiceCollection()
                .AddSingleton<IScreenManager, ScreenManager>()
                .AddSingleton<IConfigService, ConfigService>()
                .AddSingleton<IConsoleWatcher, ConsoleWatcher>()
                .AddSingleton<CermApplication>()
                .BuildServiceProvider();

            IConfigService cfg = provider.GetRequiredService<IConfigService>();
            cfg.Load();
            CermApplication app = provider.GetRequiredService<CermApplication>();
            app.Run();
        }
    }
}
