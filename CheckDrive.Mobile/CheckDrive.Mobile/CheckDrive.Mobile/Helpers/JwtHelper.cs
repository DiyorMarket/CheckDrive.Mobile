using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CheckDrive.Mobile.Helpers
{
    public static class JwtHelper
    {
        public static bool IsTokenValid(string token)
        {
            var jwtToken = ValidateOrThrow(token);

            var expirationDate = jwtToken.ValidTo;
            return expirationDate > DateTime.UtcNow;
        }

        public static string GetAccountId(string token)
        {
            var jwtToken = ValidateOrThrow(token);

            return jwtToken.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value;
        }

        public static string GetUserId(string token)
        {
            var jwtToken = ValidateOrThrow(token);

            return jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserRole(string token)
        {
            var jwtToken = ValidateOrThrow(token);

            return jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        }

        private static JwtSecurityToken ValidateOrThrow(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                throw new ArgumentException("Invalid JWT token.");
            }

            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            return jwtToken;
        }
    }
}
