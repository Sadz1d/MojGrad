namespace Market.Application.Modules.Surveys.Survey.Queries.List;

public sealed class ListSurveysQueryDto
{
    public required int Id { get; init; }
    public required string Question { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public int ResponsesCount { get; init; }   // koliko odgovora ima anketa
    public bool IsActive { get; init; }        // da li je trenutno aktivna
}
