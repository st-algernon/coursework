using Coursework.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Coursework.Web.Hubs;

[Authorize]
public class CommentsHub : Hub
{
    private readonly IMediator _mediator;
        
    public CommentsHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send(CreateCommentCommand request)
    {
        request.CurrentUserId = Context.User?.Identity?.Name;
        
        var response = await _mediator.Send(request, Context.ConnectionAborted);
        
        await Clients.All.SendAsync("Receive", response);
    }
}