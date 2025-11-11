using MediatR;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Delete;

public sealed class DeleteSurveyResponseCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
