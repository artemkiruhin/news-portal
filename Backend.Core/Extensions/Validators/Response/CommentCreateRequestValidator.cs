using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Response;

public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
{
    public CommentCreateRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Комментарий не может быть пустым")
            .MaximumLength(2000).WithMessage("Максимум 2000 символов");

        RuleFor(x => x.ReplyId)
            .NotEmpty().When(x => x.ReplyId.HasValue)
            .NotEqual(Guid.Empty).When(x => x.ReplyId.HasValue);
    }
}