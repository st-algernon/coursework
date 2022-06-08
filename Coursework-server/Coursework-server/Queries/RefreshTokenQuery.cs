using System.ComponentModel.DataAnnotations;
using Coursework_server.Data.DTOs.Responses;
using MediatR;

namespace Coursework_server.Queries;

public class RefreshTokenQuery : IRequest<AuthResponse>
{
    [Required] public string AccessToken { get; set; }
    [Required] public string RefreshToken { get; set; }
}