using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections;

namespace Kota_Palace_Admin.Hubs
{
    public class OrderHub : Hub
    {
        /*public void NotifyApp(Order order)
        {
            Clients.All.SendAsync("Order", order);
        }*/
        public void NotifyApp(Order order)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            Clients.All.SendAsync(order.BusinessId.ToString(), json);
        }
        public void UpdateOrder(Order order)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            Clients.All.SendAsync(order.CustomerId, json);

        }
    }
}
