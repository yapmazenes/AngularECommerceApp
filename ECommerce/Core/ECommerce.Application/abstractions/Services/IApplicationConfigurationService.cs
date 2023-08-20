using ECommerce.Application.DTOs.ApplicationConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IApplicationConfigurationService
    {
        List<MenuDto> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
