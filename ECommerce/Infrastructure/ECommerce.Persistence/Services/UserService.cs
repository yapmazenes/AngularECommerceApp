using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEndpointReadRepository _endpointReadRepository;
        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null) return false;

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles == null || userRoles.Count == 0) return false;

            var endpoint = await _endpointReadRepository.Table.Include(x => x.Roles).FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null) return false;

            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                {
                    if (userRole == endpointRole) return true;
                }
            }

            return false;
        }
    }
}
