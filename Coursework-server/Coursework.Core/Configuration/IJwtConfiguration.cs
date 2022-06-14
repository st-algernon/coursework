using Microsoft.IdentityModel.Tokens;

namespace Coursework.Core.Configuration;

public interface IJwtConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int AccessTokenLifetimeInMinutes { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey();
}