namespace Market.Application.Modules.Reports.Tasks.Queries.List;

public sealed class ListTasksQueryDto
{
    public required int Id { get; init; }
    public required string ReportTitle { get; init; }       // Naziv povezanog izvještaja
    public required string WorkerName { get; init; }        // Ime korisnika (radnika)
    public DateTime? AssignmentDate { get; init; }
    public DateTime? Deadline { get; init; }
    public required string TaskStatus { get; init; }
}
