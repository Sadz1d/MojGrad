using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Reports.Rating.Queries.List;

public sealed class ListRatingQuery : BasePagedQuery<ListRatingQueryDto>
{
    public int? ReportId { get; init; } // filtriranje po prijavi
    public int? UserId { get; init; }   // filtriranje po korisniku
    public int? MinRating { get; init; } // npr. 3, 4, 5...
}

