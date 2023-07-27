using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.CustomAttributes;
using ECommerce.Application.DTOs.ApplicationConfiguration;
using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class ApplicationConfigurationService : IApplicationConfigurationService
    {
        public List<MenuDto> GetAuthorizeDefinitionEndpoints(Type type)
        {
            var currentAssembly = Assembly.GetAssembly(type);

            var controllers = currentAssembly.GetTypes().Where(x => x.IsAssignableTo(typeof(ControllerBase)));
            var menus = new List<MenuDto>();

            if (controllers != null)
            {
                foreach (var controller in controllers)
                {
                    var authorizedDefinitionActions = controller.GetMethods().
                        Where(x => x.IsDefined(typeof(AuthorizeDefinitionAttribute)));

                    if (authorizedDefinitionActions != null)
                    {
                        foreach (var action in authorizedDefinitionActions)
                        {
                            var authorizeDefinition = action.GetCustomAttribute<AuthorizeDefinitionAttribute>();

                            if (authorizeDefinition != null)
                            {
                                var menu = menus.Find(x => x.Name == authorizeDefinition.Menu);
                                var httpMethodAttr = action.GetCustomAttributes().FirstOrDefault(x => x.GetType().IsAssignableTo(typeof(HttpMethodAttribute)));
                                var httpType = HttpMethods.Get;

                                if (httpMethodAttr != null)
                                {
                                    httpType = (httpMethodAttr as HttpMethodAttribute).HttpMethods.First();
                                }

                                if (menu == null)
                                {
                                    menu = new MenuDto
                                    {
                                        Name = authorizeDefinition.Menu,
                                    };

                                    menus.Add(menu);
                                }

                                var actionType = Enum.GetName(authorizeDefinition.ActionType) ?? Enum.GetName(ActionType.Reading);

                                menu.Actions.Add(new ActionDto
                                {
                                    ActionType = actionType,
                                    Definition = authorizeDefinition.Definition,
                                    HttpType = httpType,
                                    Code = $"{httpType}.{actionType}.{authorizeDefinition.Definition.Replace(" ", "")}"
                                });
                            }
                        }
                    }
                }
            }
            return menus;
        }
    }
}
