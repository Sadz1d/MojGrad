using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Rewards.AssignedRewards.Commands.Update;

public sealed class UpdateAssignedRewardCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; } // dolazi iz rute: /api/rewards/assigned/{id}
    public int? RewardId { get; set; }       // ako želiš promijeniti nagradu
    public DateTime? AssignmentDate { get; set; } // ako želiš ažurirati datum dodjele
}
