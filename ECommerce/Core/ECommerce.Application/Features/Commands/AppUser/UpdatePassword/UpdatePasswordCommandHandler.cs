using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ECommerce.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UpdatePasswordCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
                throw new PasswordChangeFailedException("Password could not verified with Password Confirm");

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user != null)
            {
                var tokenBytes = WebEncoders.Base64UrlDecode(request.ResetToken);

                request.ResetToken = Encoding.UTF8.GetString(tokenBytes);

                var result = await _userManager.ResetPasswordAsync(user, request.ResetToken, request.Password);

                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }

            return new();
        }
    }
}
