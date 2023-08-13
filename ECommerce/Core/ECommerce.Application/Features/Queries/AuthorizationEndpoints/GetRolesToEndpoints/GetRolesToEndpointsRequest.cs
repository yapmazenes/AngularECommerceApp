using MediatR;

namespace ECommerce.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints
{
    public class GetRolesToEndpointsQueryRequest : IRequest<GetRolesToEndpointsQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}
