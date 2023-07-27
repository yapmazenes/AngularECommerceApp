using ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationConfigurationController : ControllerBase
    {
        private readonly IApplicationConfigurationService _applicationConfigurationService;

        public ApplicationConfigurationController(IApplicationConfigurationService applicationConfigurationService)
        {
            _applicationConfigurationService = applicationConfigurationService;
        }

        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            return Ok(_applicationConfigurationService.GetAuthorizeDefinitionEndpoints(typeof(Program)));
        }
    }
}
