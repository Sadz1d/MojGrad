using MediatR;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Commands.Create;

public sealed class CreateActionParticipantCommand : IRequest<int>
{
    public required int ActionId { get; init; }         // ciljna volonterska akcija
    public required int UserId { get; init; }           // korisnik koji se prijavljuje
    public DateTime? RegistrationDate { get; init; }    // ako null -> UtcNow
}
