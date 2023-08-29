using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections;

namespace Kota_Palace_Admin.Hubs
{
    public class OrderHub : Hub
    {
        public void NotifyApp(Order order)
        {

            Clients.All.SendAsync("Order", order);

        }
    }
}
