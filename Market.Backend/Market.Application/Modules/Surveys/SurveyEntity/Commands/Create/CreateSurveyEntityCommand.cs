using MediatR;

namespace Market.Application.Modules.Surveys.Survey.Commands.Create;

public sealed class CreateSurveyCommand : IRequest<int>
{
    public required string Question { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}
