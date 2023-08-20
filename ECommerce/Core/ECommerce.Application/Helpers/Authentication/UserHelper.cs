using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Helpers.Authentication
{
    public class UserHelper
    {
        public static async Task UpdateRefreshToken(UserManager<AppUser> _userManager, AppUser? appUser, Token accessToken)
        {
            appUser.RefreshToken = accessToken.RefreshToken;
            appUser.RefreshTokenEndDate = accessToken.Expiration.AddMinutes(30);
            await _userManager.UpdateAsync(appUser);
        }
    }
}
