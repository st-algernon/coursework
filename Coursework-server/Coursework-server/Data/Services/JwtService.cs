using Coursework_server.Configs;
using Coursework_server.Data.Models;
using Coursework_server.Data.DTOs.Responses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Coursework_server.Queries;

namespace Coursework_server.Data.Services
{
    public class JwtService
    {
        private readonly AppDbContext _db;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtService(AppDbContext context, TokenValidationParameters validationParams)
        {
            _db = context;
            _tokenValidationParameters = validationParams;
        }

        public RefreshToken? GetRefreshToken(string refreshToken) => _db.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

        public string GenerateAccessToken(User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: JwtConfig.Issuer,
                    audience: JwtConfig.Audience,
                    notBefore: now,
                    claims: new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },
                    expires: now.Add(TimeSpan.FromMinutes(JwtConfig.AccessTokenLifetimeInMinutes)),
                    signingCredentials: new SigningCredentials(JwtConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        public RefreshToken CreateRefreshToken(User user, string accessToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.ReadJwtToken(accessToken);

            var refreshToken = new RefreshToken()
            {
                JwtId = jwt.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(JwtConfig.RefreshTokenLifetimeInDays),
                IsRevoked = false,
                Token = Guid.NewGuid().ToString()
            };

            _db.RefreshTokens.Add(refreshToken);
            _db.SaveChanges();

            return refreshToken;
        }

        public TokensVerificationResponse VerificationToken(RefreshTokenQuery tokenRequest)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var principal = jwtHandler.ValidateToken(tokenRequest.AccessToken, _tokenValidationParameters, out var validatedToken);
                
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return new TokensVerificationResponse
                        {
                            Result = false,
                            Message = "The encryption algorithm doesn't match"
                        };
                    }
                }

                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "We cannot refresh this since the access token has not expired"
                    };
                }

                var storedRefreshToken = GetRefreshToken(tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "Refresh token doesn't exist"
                    };
                }

                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "Refresh token has expired, user needs to relogin"
                    };
                }

                if (storedRefreshToken.IsUsed)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "Refresh token has been used"
                    };
                }

                if (storedRefreshToken.IsRevoked)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "Refresh token has been revoked"
                    };
                }

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedRefreshToken.JwtId != jti)
                {
                    return new TokensVerificationResponse
                    {
                        Result = false,
                        Message = "The token doesn't matched the saved token"
                    };
                }

                return new TokensVerificationResponse
                {
                    Result = true,
                    Message = "Token verification successful"
                };
            }
            catch (Exception ex)
            {
                return new TokensVerificationResponse
                {
                    Result = false,
                    Message = ex.Message
                };
            }
        }

        public void MarkRefreshTokenAsUsed(RefreshToken refreshToken)
        {
            refreshToken.IsUsed = true;

            _db.RefreshTokens.Update(refreshToken);
            _db.SaveChanges();
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}
