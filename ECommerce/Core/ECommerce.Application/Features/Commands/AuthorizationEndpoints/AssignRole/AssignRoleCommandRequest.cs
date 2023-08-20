using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.AuthorizationEndpoints.AssignRole
{
    public class AssignRoleCommandRequest : IRequest<AssignRoleCommandResponse>
    {
        public string[] Roles { get; set; }
        public string EndpointCode { get; set; }
        public Type? Type { get; set; }
        public string Menu { get; set; }
    }
}
