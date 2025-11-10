using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Media.Queries.GetById;

public sealed class GetMediaAttachmentByIdQuery : IRequest<GetMediaAttachmentByIdQueryDto>
{
    public int Id { get; init; }
}

