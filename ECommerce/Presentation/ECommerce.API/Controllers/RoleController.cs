using ECommerce.Application.Commands.Role.DeleteRole;
using ECommerce.Application.Features.Commands.Role.CreateRole;
using ECommerce.Application.Features.Commands.Role.UpdateRole;
using ECommerce.Application.Features.Queries.Role.GetRoleById;
using ECommerce.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _mediator.Send(new GetRolesQueryRequest());

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRole([FromRoute] GetRoleByIdQueryHandler getRoleByIdQueryHandler)
        {
            var response = await _mediator.Send(getRoleByIdQueryHandler);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommandRequest createRoleCommandRequest)
        {
            var response = await _mediator.Send(createRoleCommandRequest);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            var response = await _mediator.Send(updateRoleCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            var response = await _mediator.Send(deleteRoleCommandRequest);

            return Ok(response);
        }
    }
}
