using ECommerce.Application.Features.Commands.AuthorizationEndpoints.AssignRole;
using ECommerce.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointsQueryRequest getRolesToEndpointsQueryRequest)
        {
            var response = await _mediator.Send(getRolesToEndpointsQueryRequest);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AssignRoleEndpoint(AssignRoleCommandRequest assignRoleCommandRequest)
        {
            assignRoleCommandRequest.Type = typeof(Program);
            var response = await _mediator.Send(assignRoleCommandRequest);
            return Ok(response);
        }
    }
}
