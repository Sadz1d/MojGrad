namespace Market.Application.Modules.Events.EventCalendar.Queries.List;

public sealed class ListEventCalendarQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }           // Naziv događaja
    public string? Description { get; init; }           // Opis događaja
    public required DateTime EventDate { get; init; }   // Datum događaja
    public string? EventType { get; init; }             // Tip događaja
}
