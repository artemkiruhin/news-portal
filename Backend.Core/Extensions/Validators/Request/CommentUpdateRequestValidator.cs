using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Request;

public class CommentUpdateRequestValidator : AbstractValidator<CommentUpdateRequest>
{
    public CommentUpdateRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Комментарий не может быть пустым")
            .MaximumLength(2000).WithMessage("Максимум 2000 символов");
    }
}