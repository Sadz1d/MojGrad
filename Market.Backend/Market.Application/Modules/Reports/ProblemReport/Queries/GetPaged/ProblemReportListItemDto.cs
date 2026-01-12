using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged;

public sealed class ProblemReportListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = null!;
}

