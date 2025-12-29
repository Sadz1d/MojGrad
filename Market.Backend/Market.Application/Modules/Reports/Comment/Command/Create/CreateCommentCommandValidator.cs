using FluentValidation;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Comments.Commands.Create;

public sealed class CreateCommentCommandValidator
    : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.ReportId)
            .GreaterThan(0);

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Comment text is required.")
            .MaximumLength(CommentEntity.Constraints.TextMaxLength)
            .WithMessage($"Comment can be at most {CommentEntity.Constraints.TextMaxLength} characters long.");
    }
}
