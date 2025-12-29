using FluentValidation;
using Market.Application.Modules.Report.Ratings.Commands.Update;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Ratings.Commands.Update;

public sealed class UpdateRatingCommandValidator
    : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.RatingComment)
            .MaximumLength(RatingEntity.Constraints.CommentMaxLength)
            .WithMessage($"Comment can be at most {RatingEntity.Constraints.CommentMaxLength} characters long.");
    }
}
