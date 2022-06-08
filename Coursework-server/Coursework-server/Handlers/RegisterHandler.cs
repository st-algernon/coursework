using Coursework_server.Data;
using Coursework_server.Data.DTOs.Responses;
using Coursework_server.Data.Models;
using Coursework_server.Data.Services;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Coursework_server.Handlers;

internal class RegisterHandler : BaseAuthHandler, IRequestHandler<RegisterQuery, AuthResponse>
{
    public RegisterHandler(AppDbContext db, TokenValidationParameters parameters) : base (db)
    { }

    public async Task<AuthResponse> Handle(RegisterQuery request, CancellationToken cancellationToken)
    {
        var user = await Db.Users
            .FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);

        if (user != null)
        {
            throw new InvalidOperationException("A user with this email already exists");
        }

        user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashHelper.ComputeSha3Hash(request.Password)
        };

        Db.Add(user);
        await Db.SaveChangesAsync(cancellationToken);

        var accessToken = GenerateAccessToken(user);
        var refreshToken = CreateRefreshToken(user, accessToken).Token;

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}