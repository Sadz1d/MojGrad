using MediatR;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.List;

public sealed class ListVolunteerActionsQuery : BasePagedQuery<ListVolunteerActionsQueryDto>
{
    public string? Search { get; init; }        // name / description / location
    public DateTime? DateFrom { get; init; }    // filter: EventDate >=
    public DateTime? DateTo { get; init; }      // filter: EventDate <=
    public bool? OnlyUpcoming { get; init; }    // samo budući eventi
    public bool? OnlyWithFreeSlots { get; init; } // ima li slobodnih mjesta
}
