﻿using Coursework.Core.Data.ViewModels;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;
        
    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{itemId}")]
    public Task<List<CommentVm>> GetComments(Guid itemId) =>
        _mediator.Send(new GetCommentsQuery(itemId), HttpContext.RequestAborted);
}