using FluentValidation;

namespace Market.Application.Modules.Civic.WatchList.Commands.Create;

public sealed class AddToWatchListCommandValidator
    : AbstractValidator<CreateWatchListCommand>
{
    public AddToWatchListCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}