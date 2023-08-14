using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace prjEShopping.Models
{
    public class MyHub : Hub
    {
        public async Task CreateRoom(string roomId)
        {
            await Groups.Add(Context.ConnectionId, roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.Remove(Context.ConnectionId, roomId);
        }

        public async Task SendMessageToRoom(string roomId, string userName, string message)
        {
            
            await Clients.Group(roomId).ReceiveMessage(userName, message);
            
        }
    }
}