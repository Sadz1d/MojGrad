using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Commands.Update;

public sealed class UpdateVolunteerActionCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }      // iz rute: /api/volunteering/actions/{id}

    public int? VolunteerId { get; set; }         // promjena organizatora (opcionalno)
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime? EventDate { get; set; }
    public int? MaxParticipants { get; set; }
}
