using System.ComponentModel.DataAnnotations;
using Coursework_server.Data.DTOs.Responses;
using MediatR;

namespace Coursework_server.Queries;

public class LoginQuery : IRequest<AuthResponse>
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }   
}