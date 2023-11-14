using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.SignalR;

namespace Kota_Palace_Admin.Hubs
{
    public class TrackOrderHub : Hub
    {
      
        public void UpdateOrder(string order)
        {
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            Clients.All.SendAsync("thima", "data");
            
        } 
        public void UpdateOrder_(Order order)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            Clients.All.SendAsync(order.CustomerId.ToString(), json);

        }
    }
}
