using FluentValidation;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Comments.Commands.Update;

public sealed class UpdateCommentCommandValidator
    : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Comment text is required.")
            .MaximumLength(CommentEntity.Constraints.TextMaxLength)
            .WithMessage($"Comment can be at most {CommentEntity.Constraints.TextMaxLength} characters long.");
    }
}
