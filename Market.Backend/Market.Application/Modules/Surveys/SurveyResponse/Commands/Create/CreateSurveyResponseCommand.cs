using MediatR;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Create;

public sealed class CreateSurveyResponseCommand : IRequest<int>
{
    public required int SurveyId { get; init; }
    public required int UserId { get; init; }
    public required string ResponseText { get; init; }
}
