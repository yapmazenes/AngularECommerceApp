using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager,
                                       SignInManager<Domain.Entities.Identity.AppUser> signInManager,
                                       ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            appUser ??= await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if (appUser == null) throw new NotFoundUserException();

            var signInResult = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, false);

            if (signInResult.Succeeded)
            {
                return new LoginUserSuccessCommandResponse
                {
                    Token = _tokenHandler.CreateAccessToken(30)
                };
            }

            throw new AuthenticationErrorException();
        }
    }
}
