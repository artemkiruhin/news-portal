using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Request;

public class PostUpdateRequestValidator : AbstractValidator<PostUpdateRequest>
{
    public PostUpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).When(x => x.Title != null);

        RuleFor(x => x.Subtitle)
            .MaximumLength(400).When(x => x.Subtitle != null);

        RuleFor(x => x.Content)
            .MaximumLength(8000).When(x => x.Content != null);

        RuleFor(x => x.DepartmentIds)
            .ForEach(id => id.NotEmpty().NotEqual(Guid.Empty))
            .When(x => x.DepartmentIds != null);
    }
}