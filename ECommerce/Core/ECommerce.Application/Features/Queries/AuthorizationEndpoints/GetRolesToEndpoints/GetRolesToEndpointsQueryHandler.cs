using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints
{
    public class GetRolesToEndpointsQueryHandler : IRequestHandler<GetRolesToEndpointsQueryRequest, GetRolesToEndpointsQueryResponse>
    {
        private readonly IEndpointReadRepository _endpointReadRepository;

        public GetRolesToEndpointsQueryHandler(IEndpointReadRepository endpointReadRepository)
        {
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<GetRolesToEndpointsQueryResponse> Handle(GetRolesToEndpointsQueryRequest request, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointReadRepository.Table.Include(x => x.Roles).Include(x => x.Menu)
                .FirstOrDefaultAsync(x => x.Code == request.Code && x.Menu.Name == request.Menu);

            return new()
            {
                Roles = endpoint?.Roles.Select(r => r.Name) ?? null
            };
        }

    }
}
