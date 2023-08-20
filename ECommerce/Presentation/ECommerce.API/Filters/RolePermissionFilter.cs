using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ECommerce.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;

            if (!string.IsNullOrEmpty(name) && name != "enes.yapmaz")
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (descriptor == null) return;

                var authorizeDefinitionAttribute = descriptor.MethodInfo.GetCustomAttribute<AuthorizeDefinitionAttribute>();

                if (authorizeDefinitionAttribute == null) return;

                var httpMethodAttribute = descriptor.MethodInfo.GetCustomAttribute<HttpMethodAttribute>();

                var code = $"{httpMethodAttribute?.HttpMethods.First() ?? HttpMethods.Get}.{authorizeDefinitionAttribute.ActionType}.{authorizeDefinitionAttribute.Definition.Replace(" ", "")}";
                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                await next();
        }
    }
}
