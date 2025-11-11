using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Rewards.Reward.Commands.Update;

public sealed class UpdateRewardCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }   // dolazi iz rute: /api/rewards/{id}
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? MinimumPoints { get; set; }
}
