using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.AuthorizationEndpoints.AssignRole
{
    public class AssignRoleCommandResponse
    {
        public bool Succeded { get; set; }

        public AssignRoleCommandResponse(bool succeded)
        {
            Succeded = succeded;
        }

        public string Message { get; set; }
    }
}
