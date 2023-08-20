using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Helpers.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;

        public RefreshTokenLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);

            if (user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                var accessToken = _tokenHandler.CreateAccessToken(user);
                await UserHelper.UpdateRefreshToken(_userManager, user, accessToken);
                return new()
                {
                    Token = accessToken
                };
            }

            throw new NotFoundUserException();
        }
    }
}
