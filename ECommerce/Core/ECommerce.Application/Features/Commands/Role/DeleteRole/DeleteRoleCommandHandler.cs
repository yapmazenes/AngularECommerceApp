using ECommerce.Application.Features.Commands.Role.DeleteRole;
using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var appRole = await _roleManager.FindByIdAsync(request.Id);
            if (appRole == null) throw new Exception("Not Found Role");
            
            var result = await _roleManager.DeleteAsync(appRole);

            return new DeleteRoleCommandResponse
            {
                Succeeded = result.Succeeded
            };
        }
    }
}
