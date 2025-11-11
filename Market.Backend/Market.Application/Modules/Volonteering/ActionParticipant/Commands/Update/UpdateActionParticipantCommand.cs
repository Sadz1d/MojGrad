using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Update;

public sealed class UpdateActionParticipantCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }             // iz rute: /api/volunteering/action-participants/{id}
    public int? ActionId { get; set; }                   // promjena akcije (opcionalno)
    public int? UserId { get; set; }                     // promjena korisnika (opcionalno)
    public DateTime? RegistrationDate { get; set; }      // promjena datuma (opcionalno)
}
