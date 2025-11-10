using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.Tasks.Commands.Update;

public sealed class UpdateTaskCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }  // dolazi iz rute (npr. /api/reports/tasks/{id})
    public DateTime? Deadline { get; set; }
    public string? TaskStatus { get; set; }
    public bool? Completed { get; set; } // ako imaš logičku oznaku završetka (možeš izostaviti ako nema)
}
