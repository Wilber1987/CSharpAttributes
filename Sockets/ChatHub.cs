using API.Controllers;
using APPCORE.Services;
using DataBaseModel;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WLLM.Hubs.MensajeriaNotificaciones
{
    public class ChatHub : Hub
    {



        // Diccionario para mapear userId → ConnectionId
        public static readonly Dictionary<string, string> UserConnectionMap = new();

        public override Task OnConnectedAsync()
        {
            string? userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnectionMap)
                {                    
                    if (UserConnectionMap.ContainsKey(userId))
                        UserConnectionMap[userId] = Context.ConnectionId;
                    else
                        UserConnectionMap.Add(userId, Context.ConnectionId);
                }
                // Opcional: notificar a otros que este usuario se conectó
            }
            return base.OnConnectedAsync();
        }

        private string? GetUserId()
        {
            var sessionKey = Context.GetHttpContext()?.Session.GetString("sessionKey");

            var user = AuthNetCore.User(sessionKey);
            string? userId = user?.UserId.ToString();
            return userId;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string? userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnectionMap)
                {
                    UserConnectionMap.Remove(userId);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }     

    }
}