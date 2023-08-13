using ECommerce.Application.Features.Commands.Role.DeleteRole;
using MediatR;

namespace ECommerce.Application.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandRequest : IRequest<DeleteRoleCommandResponse>
    {
        public string Id { get; set; }
    }
}
