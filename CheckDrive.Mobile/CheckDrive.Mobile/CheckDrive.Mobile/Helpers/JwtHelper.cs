using System;
using System.IdentityModel.Tokens.Jwt;

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
    }
}
