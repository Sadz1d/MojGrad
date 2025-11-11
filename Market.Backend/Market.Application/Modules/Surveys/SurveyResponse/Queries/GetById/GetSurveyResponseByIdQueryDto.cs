namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.GetById;

public sealed class GetSurveyResponseByIdQueryDto
{
    public required int Id { get; init; }
    public required int SurveyId { get; init; }
    public required string SurveyQuestion { get; init; }
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required string ResponseText { get; init; }
}
