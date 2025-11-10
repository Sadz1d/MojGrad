using MediatR;

namespace Market.Application.Modules.Reports.Tasks.Queries.GetById;

public sealed class GetTaskByIdQuery : IRequest<GetTaskByIdQueryDto>
{
    public int Id { get; init; }
}
