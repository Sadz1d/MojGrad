using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Reports.Comment.Queries.GetById;

public sealed class GetCommentByIdQuery : IRequest<GetCommentByIdQueryDto>
{
    public int Id { get; init; }
}

