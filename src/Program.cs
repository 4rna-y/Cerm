using System.Collections.ObjectModel;
using Cerm.Input;
using Cerm.Lifetime;
using Cerm.Lifetime.Event;
using Cerm.Render;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;

namespace Cerm
{
    public class Program
    {
        static void Main(string[] args) =>
            new Program().Run(args);

        void Run(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                })
                .UseNLog()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHostedService<CermApplication>();
                    services.AddHostedService<InputHandler>();
                    services.AddHostedService<TerminalRenderer>();
                })
                .Build()
                .Run();
        }
    }
}