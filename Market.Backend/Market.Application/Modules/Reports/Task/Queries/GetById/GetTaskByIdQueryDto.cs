namespace Market.Application.Modules.Reports.Tasks.Queries.GetById;

public sealed class GetTaskByIdQueryDto
{
    public required int Id { get; init; }
    public required string ReportTitle { get; init; }      // Naziv izvještaja kojem task pripada
    public required string WorkerName { get; init; }       // Ime i prezime korisnika koji radi task
    public DateTime? AssignmentDate { get; init; }
    public DateTime? Deadline { get; init; }
    public required string TaskStatus { get; init; }
}
