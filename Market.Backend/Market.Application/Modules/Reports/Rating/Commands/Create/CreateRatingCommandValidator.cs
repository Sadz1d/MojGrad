using FluentValidation;
using Market.Application.Modules.Reports.Rating.Commands.Create;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Ratings.Commands.Create;

public sealed class CreateRatingCommandValidator
    : AbstractValidator<CreateRatingCommand>
{
    public CreateRatingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.ReportId)
            .GreaterThan(0);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.RatingComment)
            .MaximumLength(RatingEntity.Constraints.CommentMaxLength)
            .WithMessage($"Comment can be at most {RatingEntity.Constraints.CommentMaxLength} characters long.");
    }
}
