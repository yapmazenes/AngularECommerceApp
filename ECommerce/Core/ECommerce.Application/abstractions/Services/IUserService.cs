﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
