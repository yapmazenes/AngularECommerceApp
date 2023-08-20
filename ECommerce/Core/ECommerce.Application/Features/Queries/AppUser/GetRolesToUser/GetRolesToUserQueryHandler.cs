using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Queries.AppUser.GetRolesToUser
{
    public class GetRolesToUserQueryHandler : IRequestHandler<GetRolesToUserQueryRequest, GetRolesToUserQueryResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public GetRolesToUserQueryHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
        {
            var response = new GetRolesToUserQueryResponse();

            var user = await _userManager.FindByIdAsync(request.UserId);
            user ??= await _userManager.FindByNameAsync(request.UserId);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                response.Roles = userRoles.ToArray();
            }
            else
            {
                response.Roles = new string[] { };
            }

            return response;
        }

    }
}
