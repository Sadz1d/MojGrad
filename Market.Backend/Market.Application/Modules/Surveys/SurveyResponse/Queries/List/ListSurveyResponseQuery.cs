using MediatR;

namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.List;

public sealed class ListSurveyResponsesQuery : BasePagedQuery<ListSurveyResponsesQueryDto>
{
    public string? Search { get; init; }      // pretraga po korisniku ili tekstu odgovora
    public int? SurveyId { get; init; }       // filter po anketi
    public int? UserId { get; init; }         // filter po korisniku
}
