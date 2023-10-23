using IceSync.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.HostedServices
{
    public class DataSynchronizationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataSynchronizationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _workflowService = scope.ServiceProvider.GetRequiredService<IWorkflowService>();
                    await _workflowService.SyncWithApi();
                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                }                    
            }
        }
    }
}
