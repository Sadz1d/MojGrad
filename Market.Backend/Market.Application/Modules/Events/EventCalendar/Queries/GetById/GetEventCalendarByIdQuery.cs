using MediatR;

namespace Market.Application.Modules.Civic.Events.Queries.GetById;

public sealed class GetEventCalendarByIdQuery : IRequest<GetEventCalendarByIdQueryDto>
{
    public int Id { get; init; }
}
