namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.List;

public sealed class ListSurveyResponsesQueryDto
{
    public required int Id { get; init; }
    public required int SurveyId { get; init; }
    public required string SurveyQuestion { get; init; }
    public required string UserName { get; init; }      // Ime i prezime korisnika
    public required string ResponseText { get; init; }
}
