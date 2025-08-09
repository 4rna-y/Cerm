using System;
using Microsoft.Extensions.Hosting;

namespace Cerm.Lifetime
{
    public class CermApplication : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
