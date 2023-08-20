using ECommerce.Application.Abstractions.Hubs;
using ECommerce.Socket.WebSocket.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Socket
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection services)
        {
            services.AddTransient<IProductHubService, ProductHubService>();
            services.AddTransient<IOrderHubService, OrderHubService>();
            services.AddSignalR();
        }
    }
}
