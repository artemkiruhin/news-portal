using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Request;

public class PostCreateRequestValidator : AbstractValidator<PostCreateRequest>
{
    public PostCreateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок обязателен")
            .MaximumLength(200).WithMessage("Максимум 200 символов");

        RuleFor(x => x.Subtitle)
            .MaximumLength(400).When(x => x.Subtitle != null);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Содержание обязательно")
            .MaximumLength(8000).WithMessage("Максимум 8000 символов");

        RuleFor(x => x.DepartmentIds)
            .NotEmpty().WithMessage("Укажите минимум один отдел")
            .ForEach(id => id.NotEmpty().NotEqual(Guid.Empty));
    }
}