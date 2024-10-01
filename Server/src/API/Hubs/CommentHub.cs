using Microsoft.AspNetCore.SignalR;
using WebShop.src.Domain.DTOs;

namespace WebShop.src.API.Hubs;

public class CommentHub(ILogger<CommentHub> logger) : Hub
{
    private readonly ILogger<CommentHub> _logger = logger;
    public async Task LeaveYourComment(CommentHubDTO comment){
        try
        {
            _logger.LogInformation("Received comment: {@Comment}", comment);
            await Clients.All.SendAsync("ShareComment", comment);
        }
        catch (Exception err)
        {
            _logger.LogError(err, "Error sending comment");
            throw new Exception("Exception: " + err.Message);
        }
    }

    public async Task Try(string message){
        await Clients.All.SendAsync("Message", message);
    }
}