using System.ComponentModel.DataAnnotations;
using Coursework.Core.Data.DTOs.Responses;
using MediatR;

namespace Coursework.Core.Queries;

public class LoginQuery : IRequest<AuthResponse>
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }   
}