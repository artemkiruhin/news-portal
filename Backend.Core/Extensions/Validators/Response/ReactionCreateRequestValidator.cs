using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Response;

public class ReactionCreateRequestValidator : AbstractValidator<ReactionCreateRequest>
{
    public ReactionCreateRequestValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Недопустимый тип реакции");
    }
}