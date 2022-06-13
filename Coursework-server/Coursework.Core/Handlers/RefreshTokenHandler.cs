using Coursework.Core.Configuration;
using Coursework.Core.Data;
using Coursework.Core.Data.DTOs.Responses;
using Coursework.Core.Handlers.Base;
using Coursework.Core.Queries;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Coursework.Core.Handlers;

internal class RefreshTokenHandler : BaseAuthHandler, IRequestHandler<RefreshTokenQuery, AuthResponse>
{
    private readonly TokenValidationParameters _validationParameters;
    
    public RefreshTokenHandler(
        AppDbContext db, 
        TokenValidationParameters validationParameters,
        IJwtConfiguration jwtConfiguration
    ) : base(db, jwtConfiguration)
    {
        _validationParameters = validationParameters;
    }
    
    public Task<AuthResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var verificationResult = VerificationToken(request, _validationParameters);

        if (verificationResult.Result == false)
        {
            throw new InvalidOperationException(verificationResult.Message);
        }

        var refreshToken = GetRefreshToken(request.RefreshToken);
        var user = Db.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);

        MarkRefreshTokenAsUsed(refreshToken);

        var accessToken = GenerateAccessToken(user);
        var newRefreshToken = CreateRefreshToken(user, accessToken).Token;

        return Task.FromResult(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        });
    }
}