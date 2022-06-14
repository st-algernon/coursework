using Coursework.Core.Configuration;
using Coursework.Core.Data;
using Coursework.Core.Data.DTOs.Responses;
using Coursework.Core.Data.Models;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class RegisterHandler : BaseAuthHandler, IRequestHandler<RegisterQuery, AuthResponse>
{
    public RegisterHandler(AppDbContext db, IJwtConfiguration jwtConfiguration) : base (db, jwtConfiguration)
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