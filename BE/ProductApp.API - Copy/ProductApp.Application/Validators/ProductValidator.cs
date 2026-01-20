using FluentValidation;
using ProductApp.Application.DTOs.Product;

namespace ProductApp.Application.Validators;

public class ProductValidator : AbstractValidator<ProductCreateDto>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
            .MaximumLength(100).WithMessage("Tên không được quá 100 ký tự");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
    }
}