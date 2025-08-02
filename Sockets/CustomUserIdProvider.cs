using API.Controllers;
using Microsoft.AspNetCore.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        var sessionKey = connection.GetHttpContext()?.Session.GetString("sessionKey");
        var user = AuthNetCore.User(sessionKey);
        return user?.UserId.ToString() ?? string.Empty;
    }
}