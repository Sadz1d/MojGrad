using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Application.Modules.Reports.Comment.Queries.List;

namespace Market.Application.Modules.Reports.Comment.Queries.List;

public sealed class ListCommentQuery : BasePagedQuery<ListCommentQueryDto>
{
    public int? ReportId { get; init; }   // filtriranje po prijavi
    public int? UserId { get; init; }     // filtriranje po autoru
    public string? Search { get; init; }  // pretraga po tekstu
}

