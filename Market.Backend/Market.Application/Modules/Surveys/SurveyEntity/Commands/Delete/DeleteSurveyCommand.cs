using MediatR;

namespace Market.Application.Modules.Surveys.Survey.Commands.Delete;

public sealed class DeleteSurveyCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
