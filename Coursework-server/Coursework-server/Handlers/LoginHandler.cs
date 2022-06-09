using Coursework_server.Data;
using Coursework_server.Data.DTOs.Responses;
using Coursework_server.Handlers.Base;
using Coursework_server.Helpers;
using Coursework_server.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Coursework_server.Handlers;

internal class LoginHandler : BaseAuthHandler, IRequestHandler<LoginQuery, AuthResponse>
{
    public LoginHandler(AppDbContext db) : base(db)
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