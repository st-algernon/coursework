using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Coursework_server.Configs
{
    public class JwtConfig
    {
        public const string Issuer = "CourseworkServer";
        public const string Audience = "CourseworkClient";
        private const string Key = "nnuFFSju3Hh0Eamzeey3kznqbvqyYK8Q";
        public const int AccessTokenLifetimeInMinutes = 5;
        public const int RefreshTokenLifetimeInDays = 15;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
