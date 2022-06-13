using System.ComponentModel.DataAnnotations;
using Coursework.Core.Data.DTOs.Responses;
using MediatR;

namespace Coursework.Core.Queries;

public class RefreshTokenQuery : IRequest<AuthResponse>
{
    [Required] public string AccessToken { get; set; }
    [Required] public string RefreshToken { get; set; }
}