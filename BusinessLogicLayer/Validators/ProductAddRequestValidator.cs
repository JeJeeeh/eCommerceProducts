using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        // Product Name
        RuleFor(temp => temp.ProductName)
            .NotEmpty().WithMessage("Product name is required.");
        
        // Category
        RuleFor(temp => temp.Category)
            .IsInEnum().WithMessage("Invalid category.");
        
        // Unit Price
        RuleFor(temp => temp.UnitPrice)
            .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit price should be between 0 to {double.MaxValue}");
        
        // Quantity Stock
        RuleFor(temp => temp.QuantityInStock)
            .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity should be between 0 to {int.MaxValue}");
    }
}