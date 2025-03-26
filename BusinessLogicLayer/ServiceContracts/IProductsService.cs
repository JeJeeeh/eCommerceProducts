using System.Linq.Expressions;
using BusinessLogicLayer.DTO;
using DataAccessLayer.entities;

namespace BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    Task<List<ProductResponse?>> GetProducts();
    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> expression);
    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> expression);
    Task<ProductResponse?> AddProduct(ProductAddRequest request);
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest request);
    Task<bool> DeleteProduct(Guid productId);
}