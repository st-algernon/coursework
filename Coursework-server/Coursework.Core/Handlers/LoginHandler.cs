using Coursework.Core.Configuration;
using Coursework.Core.Data;
using Coursework.Core.Data.DTOs.Responses;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Helpers;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Handlers;

internal class LoginHandler : BaseAuthHandler, IRequestHandler<LoginQuery, AuthResponse>
{
    public LoginHandler(AppDbContext db, IJwtConfiguration jwtConfiguration) : base(db, jwtConfiguration)
    { }

    public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var hashedPassword = HashHelper.ComputeSha3Hash(request.Password);
        var user = await Db.Users
            .FirstOrDefaultAsync(a => a.Email == request.Email && a.Password == hashedPassword, cancellationToken); 
        
        if (user == null)
        {
            throw new InvalidOperationException("Account doesn't exist");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = CreateRefreshToken(user, accessToken).Token;

        return new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}