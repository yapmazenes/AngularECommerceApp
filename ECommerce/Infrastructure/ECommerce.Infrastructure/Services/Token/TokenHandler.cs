using ECommerce.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int expirationMinute)
        {
            var token = new Application.DTOs.Token();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddMinutes(expirationMinute);

            var securityToken = new JwtSecurityToken(
                                        issuer: _configuration["Token:Issuer"],
                                        audience: _configuration["Token:Audience"],
                                        expires: token.Expiration,
                                        notBefore: DateTime.UtcNow, //Token suresi ne zaman devreye girsin
                                        signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
