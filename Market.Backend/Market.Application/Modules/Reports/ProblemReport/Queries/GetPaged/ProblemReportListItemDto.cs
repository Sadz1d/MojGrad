// Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged.ProblemReportListItemDto
namespace Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged
{
    public sealed class ProblemReportListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime CreationDate => CreatedAt; // Alias za frontend
        public string? Location { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StatusName => Status; // Alias za frontend
        public int CommentsCount { get; set; }
        public int TasksCount { get; set; }
        public int RatingsCount { get; set; }
        public string? ShortDescription { get; set; }
    }
}