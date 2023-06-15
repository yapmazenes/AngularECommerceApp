using ECommerce.Application.Abstractions.Token;
using ECommerce.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int expirationMinute, AppUser appUser)
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
                                        signingCredentials: signingCredentials,
                                        claims: new List<Claim> { new(ClaimTypes.Name, appUser.UserName) }
                                        );

            var tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public Application.DTOs.Token CreateAccessToken(AppUser appUser)
        {
            return CreateAccessToken(int.Parse(_configuration["Token:LifetimeMinute"] ?? "30"), appUser);
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
