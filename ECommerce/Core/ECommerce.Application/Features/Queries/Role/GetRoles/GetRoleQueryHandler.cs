using ECommerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetRolesQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles;
            IQueryable<AppRole> rolesQuery = null;
            if (request.Page != -1 && request.Size != -1)
                rolesQuery = query.Skip(request.Page * request.Size).Take(request.Size);
            else
                rolesQuery = query;

            return new() { Datas = rolesQuery.Select(r => new { r.Id, r.Name }), TotalCount = query.Count() };
        }
    }
}
