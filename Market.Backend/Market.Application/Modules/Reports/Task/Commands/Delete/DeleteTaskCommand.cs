using MediatR;

namespace Market.Application.Modules.Reports.Tasks.Commands.Delete;

public sealed class DeleteTaskCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}
