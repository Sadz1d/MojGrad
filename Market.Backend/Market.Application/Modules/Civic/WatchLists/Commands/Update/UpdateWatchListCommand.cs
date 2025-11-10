using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Civic.WatchList.Commands.Update
{
    public sealed class UpdateWatchListCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }     // Id dolazi iz rute

        public int? CategoryId { get; set; }   // opcionalno; ako null -> ne diramo kategoriju
        public DateTime? DateAdded { get; set; } // opcionalno; ako null -> ne diramo datum
    }
}
