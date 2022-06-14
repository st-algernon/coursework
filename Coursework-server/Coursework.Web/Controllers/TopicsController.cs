using Coursework.Core.Data.Models;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TopicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<List<Topic>> GetTopics() => _mediator.Send(new GetTopicsQuery(), HttpContext.RequestAborted);
}