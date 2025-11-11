using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Media.MediaLink.Commands.Create;

public sealed class CreateMediaLinkCommand : IRequest<int>
{
    public required int MediaId { get; init; }
    public required string EntityType { get; init; } // <= 100
    public required int EntityId { get; init; }
}

