

namespace Market.Application.Modules.Reports.Tasks.Queries.List;

public sealed class ListTasksQuery : BasePagedQuery<ListTasksQueryDto>
{
    public string? Search { get; init; }          // Pretraga po statusu, korisniku itd.
    public bool? OnlyCompleted { get; init; }     // Filter po završenim taskovima (TaskStatus = "Completed")
    public int? ReportId { get; init; }           // Filter po izvještaju
}
