using ECommerce.Socket.WebSocket.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ECommerce.Socket.Extensions
{
    public static class HubExtension
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/products-hub");
        }
    }
}
