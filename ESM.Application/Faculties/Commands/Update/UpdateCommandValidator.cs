using FluentValidation;

namespace ESM.Application.Faculties.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(e => e.Request.Name).NotEmpty();
    }
}