using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Reports.Rating.Queries.GetById;

public sealed class GetRatingByIdQuery : IRequest<GetRatingByIdQueryDto>
{
    public int Id { get; init; }
}

