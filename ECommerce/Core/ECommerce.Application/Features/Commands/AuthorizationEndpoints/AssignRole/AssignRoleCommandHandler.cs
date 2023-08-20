using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.AuthorizationEndpoints.AssignRole
{
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
    {
        private readonly IApplicationConfigurationService _applicationConfigurationService;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IEndpointWriteRepository _endpointWriteRepository;
        private readonly IMenuReadRepository _menuReadRepository;
        private readonly IMenuWriteRepository _menuWriteRepository;
        private readonly RoleManager<AppRole> _roleManager;

        public AssignRoleCommandHandler(IApplicationConfigurationService applicationConfigurationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
        {
            _applicationConfigurationService = applicationConfigurationService;
            _endpointReadRepository = endpointReadRepository;
            _endpointWriteRepository = endpointWriteRepository;
            _menuReadRepository = menuReadRepository;
            _menuWriteRepository = menuWriteRepository;
            _roleManager = roleManager;
        }

        public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var menu = await _menuReadRepository.GetSingleAsync(m => m.Name == request.Menu);

            if (menu == null)
            {
                menu = new Domain.Entities.Menu() { Id = Guid.NewGuid(), Name = request.Menu };
                await _menuWriteRepository.AddAsync(menu);
                await _endpointWriteRepository.SaveAsync();
            }

            var endPoint = await _endpointReadRepository.Table.Include(x => x.Menu).Include(x => x.Roles).FirstOrDefaultAsync(x => x.Code == request.EndpointCode && x.Menu.Name == request.Menu);

            if (endPoint == null)
            {
                var action = _applicationConfigurationService.GetAuthorizeDefinitionEndpoints(request.Type)?.FirstOrDefault(x => x.Name == request.Menu)?.Actions.FirstOrDefault(x => x.Code == request.EndpointCode);

                endPoint = new Domain.Entities.Endpoint()
                {
                    Id = Guid.NewGuid(),
                    ActionType = action.ActionType,
                    Code = action.Code,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    MenuId = menu.Id
                };

                await _endpointWriteRepository.AddAsync(endPoint);

                await _endpointWriteRepository.SaveAsync();
            }

            foreach (var role in endPoint.Roles)
                endPoint.Roles.Remove(role);

            var appRoles = await _roleManager.Roles.Where(x => request.Roles.Contains(x.Name)).ToListAsync();

            foreach (var appRole in appRoles)
                endPoint.Roles.Add(appRole);

            await _endpointWriteRepository.SaveAsync();

            return new(true);

        }

    }
}
