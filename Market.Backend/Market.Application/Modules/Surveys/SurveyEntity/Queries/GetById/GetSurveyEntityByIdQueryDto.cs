namespace Market.Application.Modules.Surveys.Survey.Queries.GetById;

public sealed class GetSurveyByIdQueryDto
{
    public required int Id { get; init; }
    public required string Question { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public int ResponsesCount { get; init; }
    public bool IsActive { get; init; }
}
