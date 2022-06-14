using System.Text;
using Coursework.Core.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Coursework.Console.Configuration;

public class JwtConfig : IJwtConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int AccessTokenLifetimeInMinutes { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
}