using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;

namespace BusinessLogicLayer.Services;

public class ProductService(
    IValidator<ProductAddRequest> addRequestValidator, 
    IValidator<ProductUpdateRequest> updateRequestValidator,
    IMapper mapper,
    IProductsRepository productsRepository
    ) : IProductsService
{
    public async Task<List<ProductResponse?>> GetProducts()
    {
        var products = await productsRepository.GetProducts();

        var response = mapper.Map<IEnumerable<ProductResponse?>>(products);

        return response.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> expression)
    {
        var products = await productsRepository.GetProductsByCondition(expression);
        
        var response = mapper.Map<IEnumerable<ProductResponse?>>(products);

        return response.ToList();
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> expression)
    {
        var product = await productsRepository.GetProductByCondition(expression);

        if (product == null)
        {
            return null;
        }

        var response = mapper.Map<ProductResponse>(product);

        return response;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = await addRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ",
                validationResult.Errors.Select(temp => temp.ErrorMessage));

            throw new ArgumentException(errors);
        }

        var product = mapper.Map<Product>(request);

        var newProduct = await productsRepository.AddProduct(product);

        if (newProduct == null)
        {
            return null;
        }
        
        var response = mapper.Map<ProductResponse>(newProduct);

        return response;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest request)
    {
        var existingProduct = await productsRepository.GetProductByCondition(temp => temp.ProductID == request.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid product Id.");
        }

        var validationResult = await updateRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ",
                validationResult.Errors.Select(temp => temp.ErrorMessage));

            throw new ArgumentException(errors);
        }

        var product = mapper.Map<Product>(request);

        var newProduct = await productsRepository.UpdateProduct(product);

        var response = mapper.Map<ProductResponse>(newProduct);

        return response;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var product = await productsRepository.GetProductByCondition(temp => temp.ProductID == productId);

        if (product == null)
        {
            return false;
        }

        var response = await productsRepository.DeleteProduct(productId);

        return response;
    }
}