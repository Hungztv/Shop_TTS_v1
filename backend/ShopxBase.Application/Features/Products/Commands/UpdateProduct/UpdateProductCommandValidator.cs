using FluentValidation;

namespace ShopxBase.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id sản phẩm phải lớn hơn 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
            .MinimumLength(4).WithMessage("Tên sản phẩm phải có ít nhất 4 ký tự")
            .MaximumLength(100).WithMessage("Tên sản phẩm không được vượt quá 100 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug không được để trống")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug chỉ chứa chữ thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả sản phẩm không được để trống")
            .MinimumLength(10).WithMessage("Mô tả phải có ít nhất 10 ký tự")
            .MaximumLength(5000).WithMessage("Mô tả không được vượt quá 5000 ký tự");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá bán phải lớn hơn 0")
            .LessThanOrEqualTo(decimal.MaxValue).WithMessage("Giá bán không hợp lệ");

        RuleFor(x => x.CapitalPrice)
            .GreaterThan(0).WithMessage("Giá vốn phải lớn hơn 0")
            .LessThan(x => x.Price).WithMessage("Giá vốn phải nhỏ hơn giá bán");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Số lượng không được âm");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Ảnh sản phẩm không được để trống")
            .MaximumLength(500).WithMessage("Đường dẫn ảnh không được vượt quá 500 ký tự");

        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("BrandId phải lớn hơn 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId phải lớn hơn 0");
    }
}
