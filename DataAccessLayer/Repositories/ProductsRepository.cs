using System.Linq.Expressions;
using DataAccessLayer.context;
using DataAccessLayer.entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductsRepository(ApplicationDbContext applicationDbContext) : IProductsRepository
{
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await applicationDbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await applicationDbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await applicationDbContext.Products.FirstOrDefaultAsync(conditionExpression);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        applicationDbContext.Products.Add(product);
        await applicationDbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        var existingProduct = await applicationDbContext.Products
            .FirstOrDefaultAsync(x => x.ProductID == product.ProductID);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;

        await applicationDbContext.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var existingProduct = await applicationDbContext.Products.FirstOrDefaultAsync(product => product.ProductID == productId);

        if (existingProduct == null)
        {
            return false;
        }

        applicationDbContext.Products.Remove(existingProduct);
        var affectedRows = await applicationDbContext.SaveChangesAsync();

        return affectedRows > 0;
    }
}