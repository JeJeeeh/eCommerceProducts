﻿using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductToProductResponseMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddScoped<IProductsService, ProductService>();
        
        return services;
    }
}