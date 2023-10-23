using IceSync.Infrastructure.Authentication;
using IceSync.Infrastructure.HostedServices;
using IceSync.Infrastructure.Services;
using IceSync.Infrastructure.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceSync.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRefitClients(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var uniLoaderApiConfiguration = configuration.GetSection(nameof(UniLoaderApiConfiguration)).Get<UniLoaderApiConfiguration>();

            services.AddSingleton<TokenService>();

            services
                .AddRefitClient<IUniLoaderAuthApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(uniLoaderApiConfiguration.Url);
                });

            services
                .AddRefitClient<IUniLoaderApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(uniLoaderApiConfiguration.Url);
                })
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var authApi = sp.GetRequiredService<IUniLoaderAuthApi>();
                    var tokenService = sp.GetRequiredService<TokenService>();

                    return new AuthenticatedHttpClientHandler(
                        authApi,
                        uniLoaderApiConfiguration,
                        tokenService
                    );
                });

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddTransient<IWorkflowService, WorkflowService>();
            services.AddHostedService<DataSynchronizationService>();

            return services;
        }
    }
}
