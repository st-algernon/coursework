using Coursework_server.Data.DTOs.Responses;
using Coursework_server.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coursework_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
        private readonly IMediator _mediator;
        
		public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public Task<AuthResponse> Login(LoginQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);

        [HttpPost("register")]
        public Task<AuthResponse> Register(RegisterQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);

        [HttpPut("refresh")]
        public Task<AuthResponse> RefreshToken([FromBody] RefreshTokenQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);
	}
}