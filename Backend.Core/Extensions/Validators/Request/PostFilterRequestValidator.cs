using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Request;

public class PostFilterRequestValidator : AbstractValidator<PostFilterRequest>
{
    public PostFilterRequestValidator()
    {
        RuleFor(x => x.TitleContains)
            .MaximumLength(100).When(x => x.TitleContains != null);

        RuleFor(x => x.ContentContains)
            .MaximumLength(100).When(x => x.ContentContains != null);
    }
}