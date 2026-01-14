// Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged.GetPagedProblemReportsQuery
using MediatR;
using Market.Application.Common.Pagination;

namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged
{
    public sealed class GetPagedProblemReportsQuery
        : IRequest<PagedResult<ProblemReportListItemDto>>
    {
        // Filter properties
        public string? Search { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? StatusId { get; set; }

        // Pagination
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        // Sorting
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "desc";
    }
}