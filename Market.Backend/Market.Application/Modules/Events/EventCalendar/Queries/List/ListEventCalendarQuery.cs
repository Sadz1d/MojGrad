using Market.Domain.Common;

namespace Market.Application.Modules.Events.EventCalendar.Queries.List;

public sealed class ListEventCalendarQuery : BasePagedQuery<ListEventCalendarQueryDto>
{
    public string? Search { get; init; }
    public bool? OnlyUpcoming { get; init; }  // filter za buduće događaje, možeš prilagoditi
}
