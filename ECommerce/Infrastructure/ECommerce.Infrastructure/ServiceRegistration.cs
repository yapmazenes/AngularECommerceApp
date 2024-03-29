﻿using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Storage;
using ECommerce.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IApplicationConfigurationService, ApplicationConfigurationService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
    }
}
