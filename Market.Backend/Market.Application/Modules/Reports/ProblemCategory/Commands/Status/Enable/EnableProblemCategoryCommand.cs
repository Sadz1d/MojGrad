using MediatR;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Enable;

public sealed class EnableProblemCategoryCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
