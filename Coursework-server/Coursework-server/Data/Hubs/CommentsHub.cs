using Coursework_server.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Coursework_server.Data.Hubs;

[Authorize]
public class CommentsHub : Hub
{
    private readonly IMediator _mediator;
        
    public CommentsHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Send(CreateCollectionCommand request)
    {
        request.CurrentUserId = Context.User?.Identity?.Name;
        
        _mediator.Send(request, Context.ConnectionAborted);
    }
}