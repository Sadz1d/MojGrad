using MediatR;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Disable;

public sealed class DisableProblemCategoryCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
