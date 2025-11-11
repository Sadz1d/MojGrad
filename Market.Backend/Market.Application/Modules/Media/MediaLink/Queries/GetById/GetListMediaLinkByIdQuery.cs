using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Media.MediaLink.Queries.GetById;

public sealed class GetMediaLinkByIdQuery : IRequest<GetMediaLinkByIdQueryDto>
{
    public int Id { get; init; }
}

