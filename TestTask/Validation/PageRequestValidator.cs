using FluentValidation;
using TestTask.PostingsClient.Contracts;

namespace TestTask.Validation
{
    public class PageRequestValidator : AbstractValidator<PageRequest>
    {
        public PageRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(-1).WithMessage("Page number must be greater than or equal to 0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0");
        }
    }
}
