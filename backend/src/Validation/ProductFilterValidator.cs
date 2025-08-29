using FluentValidation;
using BackendApi.DTOs.Product;

namespace BackendApi.Validation
{
    public class ProductFilterValidator : AbstractValidator<ProductFilterDto>
    {
        public ProductFilterValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
            // Add more filter validations as needed
        }
    }
}
