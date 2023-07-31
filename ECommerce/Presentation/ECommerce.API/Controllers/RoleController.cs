using ECommerce.Application.Commands.Role.DeleteRole;
using ECommerce.Application.CustomAttributes;
using ECommerce.Application.Features.Commands.Role.CreateRole;
using ECommerce.Application.Features.Commands.Role.UpdateRole;
using ECommerce.Application.Features.Queries.Role.GetRoleById;
using ECommerce.Application.Features.Queries.Role.GetRoles;
using ECommerce.Domain.Consts;
using ECommerce.Domain.Enums;
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
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest getRolesQueryRequest)
        {
            var response = await _mediator.Send(getRolesQueryRequest);

            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Role By Id", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRole([FromRoute] GetRoleByIdQueryHandler getRoleByIdQueryHandler)
        {
            var response = await _mediator.Send(getRoleByIdQueryHandler);

            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> Create(CreateRoleCommandRequest createRoleCommandRequest)
        {
            var response = await _mediator.Send(createRoleCommandRequest);

            return Ok(response);
        }

        [HttpPut]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> Update(UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            var response = await _mediator.Send(updateRoleCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> Delete([FromRoute] DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            var response = await _mediator.Send(deleteRoleCommandRequest);

            return Ok(response);
        }
    }
}
