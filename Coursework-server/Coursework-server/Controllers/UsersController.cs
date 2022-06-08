using Coursework_server.Data.ViewModels;
using Coursework_server.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursework_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
        private readonly IMediator _mediator;

		public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

		[HttpGet("current")]
		[Authorize(Roles = "User")]
		public Task<UserVm> GetCurrentUser() =>
            _mediator.Send(new GetCurrentUserQuery(HttpContext.User.Identity?.Name), HttpContext.RequestAborted);

        [HttpGet("{id}")]
        public Task<UserVm> GetUserById(Guid id) =>
            _mediator.Send(new GetUserQuery(id), HttpContext.RequestAborted);
        
        [HttpGet("search/{query}")]
		public Task<List<UserVm>> SearchUsers(string? query) =>
            _mediator.Send(new SearchUsersQuery(query ?? string.Empty), HttpContext.RequestAborted);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public Task<List<UserVm>> GetUsers([FromQuery] GetUsersQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);

        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public Task<int> GetUsersCount() =>
            _mediator.Send(new GetUsersCountQuery(), HttpContext.RequestAborted);

        [HttpPut("block/{id}")]
        [Authorize(Roles = "Admin")]
        public Task BlockUser(Guid id) =>
            _mediator.Send(new BlockUserQuery(id), HttpContext.RequestAborted);

        [HttpPut("unblock/{id}")]
        [Authorize(Roles = "Admin")]
        public Task UnblockUser(Guid id) =>
            _mediator.Send(new UnblockUserQuery(id), HttpContext.RequestAborted);

        [HttpDelete("remove/{id}")]
        [Authorize(Roles = "Admin")]
        public Task RemoveUser(Guid id) =>
            _mediator.Send(new RemoveUserQuery(id), HttpContext.RequestAborted);

        [HttpPut("add-admin/{id}")]
        [Authorize(Roles = "Admin")]
        public Task AddAdmin(Guid id) => 
            _mediator.Send(new AddAdminQuery(id), HttpContext.RequestAborted);
	}
}