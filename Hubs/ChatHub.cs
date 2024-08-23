//using Humanizer;
//using Microsoft.AspNetCore.SignalR;

//namespace Inventory_PLM.Hubs
//{
//    public class ChatHub : Hub
//    {
//        public override async Task OnConnectedAsync()
//        {
//            await Clients.All.SendAsync("ReceiveMessage", "Received the Message: Another connection has been added.");
//        }

//        public async Task SendMessage(string message)
//        {
//            //To send a message to the connected client
//            await Clients.Client(Context.ConnectionId)
//                .SendAsync("ReceiveMessage", $"Received the Message: {message}");
//        }
//    }
//}
