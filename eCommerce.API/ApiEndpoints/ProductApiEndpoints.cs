using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using MySqlX.XDevAPI.Common;

namespace eCommerce.API.APIEndpoints;

public static class ProductApiEndpoints
{
    public static IEndpointRouteBuilder MapProductApiEndpoints(this IEndpointRouteBuilder app)
    {
        // [GET] /api/products
        app.MapGet("/api/products", async (IProductsService productsService) =>
        {
            var response = await productsService.GetProducts();

            return Results.Ok(response);
        });
        
        // [GET] /api/products/search/product-id/{productId}
        app.MapGet("/api/products/search/product-id/{productId:guid}", async (
            IProductsService productsService, 
            Guid productId
            ) =>
        {
            var response = await productsService.GetProductByCondition(temp => temp.ProductID == productId);
            return response == null ? Results.NotFound() : Results.Ok(response);
        });
        
        // [GET] /api/products/search/{searchString}
        app.MapGet("/api/products/search/{searchString}", async (
            IProductsService productsService,
            string searchString
        ) =>
        {
            var productsByName =
                await productsService.GetProductsByCondition(temp => temp.ProductName
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var productsByCategory =
                await productsService.GetProductsByCondition(temp => temp.Category
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var response = productsByName.Union(productsByCategory);

            return Results.Ok(response);
        });
        
        // [POST] /api/products
        app.MapPost("/api/products", async (
            IProductsService productsService,
            IValidator<ProductAddRequest> validator,
            ProductAddRequest request
        ) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp.Select(
                        err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            var response = await productsService.AddProduct(request);
            return response == null ? Results.Problem("Error in adding product.") : 
                Results.Created($"/api/products/search/product-id/{response.ProductID}", response);
        });
        
        // [PUT] /api/products
        app.MapPut("/api/products", async (
            IProductsService productsService,
            IValidator<ProductUpdateRequest> validator,
            ProductUpdateRequest request
        ) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp.Select(
                        err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            var response = await productsService.UpdateProduct(request);
            return response == null ? Results.Problem("Error in updating product.") : 
                Results.Ok(response);
        });
        
        // [DELETE] /api/products/{productId}
        app.MapDelete("/api/products/{productId:guid}", async (
            IProductsService productsService,
            Guid productId
            ) =>
        {
            var isDeleted = await productsService.DeleteProduct(productId);
            return !isDeleted ? Results.Problem("Error in deleting product.") : Results.Ok();
        });
        
        return app;
    }
}