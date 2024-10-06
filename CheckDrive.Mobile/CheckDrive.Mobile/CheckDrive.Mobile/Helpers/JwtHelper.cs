using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CheckDrive.Mobile.Helpers
{
    public static class JwtHelper
    {
        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return true;
            }

            var expirationDate = jwtToken.ValidTo;
            return expirationDate < DateTime.UtcNow;
        }

        public static string ExtractAccountIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Cannot extract id from empty string.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            return jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string ExtractRoleFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken is null)
            {
                throw new InvalidOperationException("Could not parse JWT token.");
            }

            return jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        }
    }
}
