using ESM.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ESM.Application.Hubs;

public sealed class AppHub : Hub
{
    private readonly ICurrentUserService _currentUserService;

    public AppHub(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task SendMessageImportExamination(string examinationId, string examinationName, int percentage)
    {
        string? userId = _currentUserService.UserId;
        if (userId is not null)
        {
            await Clients.User(userId).SendAsync(HubMessage.BroadcastImportExamination, percentage);
        }
    }
}