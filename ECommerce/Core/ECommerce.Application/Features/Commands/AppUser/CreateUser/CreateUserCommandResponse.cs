using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandResponse
    {
        public bool Succeded { get; set; }

        public CreateUserCommandResponse(bool succeded)
        {
            Succeded = succeded;
        }

        public string Message { get; set; }
    }
}
