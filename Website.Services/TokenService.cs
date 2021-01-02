using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Website.Models.Account;

namespace Website.Services
{
    // grab jwt from appsettings.Json
    public class TokenService : ITokenService
    {
        // single string is used to encrpt and decrypt the key
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        
        // tying the variables to the variables in appsettings.json
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            _issuer = config["Jwt:Issuer"];
        }
        public string CreateToken(ApplicationUserIdentity user)
        {
            var claims = new List<Claim>
            {
                // key value pair
                new Claim(JwtRegisteredClaimNames.NameId, user.ApplicationUserId.ToString()),
                // key value pair
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            // sign our key using SHA-512 cryptographic hash functions
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // token will expire after 30 minutes, will require them to log back in
            var token = new JwtSecurityToken(
                _issuer,
                _issuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
