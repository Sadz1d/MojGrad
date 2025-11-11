using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Media.MediaLinks.Commands.Update;

public sealed class UpdateMediaLinkCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }     // iz rute
    public int? MediaId { get; set; }            // opcionalno
    public string? EntityType { get; set; }      // opcionalno
    public int? EntityId { get; set; }           // opcionalno
}

