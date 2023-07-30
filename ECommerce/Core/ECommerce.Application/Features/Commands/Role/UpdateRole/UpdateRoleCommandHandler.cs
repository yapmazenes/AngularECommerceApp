using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace ECommerce.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public UpdateRoleCommandHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id) ?? throw new Exception("Not Found Role");
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            return new UpdateRoleCommandResponse
            {
                Succeeded = result.Succeeded,
            };
        }
    }
}
