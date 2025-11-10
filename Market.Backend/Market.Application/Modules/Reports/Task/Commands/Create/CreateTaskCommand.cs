using MediatR;

namespace Market.Application.Modules.Reports.Tasks.Commands.Create;

public sealed class CreateTaskCommand : IRequest<int>
{
    public required int ReportId { get; init; }       // Povezan izvještaj
    public required int WorkerId { get; init; }       // Korisnik koji radi task
    public DateTime? AssignmentDate { get; init; }    // Kada je dodijeljen
    public DateTime? Deadline { get; init; }          // Rok za završetak
    public required string TaskStatus { get; init; }  // Npr. "Pending", "InProgress", "Completed"
}
