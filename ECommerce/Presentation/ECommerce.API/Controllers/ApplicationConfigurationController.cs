using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.CustomAttributes;
using ECommerce.Domain.Consts;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationConfigurationController : ControllerBase
    {
        private readonly IApplicationConfigurationService _applicationConfigurationService;

        public ApplicationConfigurationController(IApplicationConfigurationService applicationConfigurationService)
        {
            _applicationConfigurationService = applicationConfigurationService;
        }

        [HttpGet()]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.ApplicationConfigurations,
                             ActionType = ActionType.Reading, Definition = "Get Authorize Definition Endpoints")]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            return Ok(_applicationConfigurationService.GetAuthorizeDefinitionEndpoints(typeof(Program)));
        }
    }
}
