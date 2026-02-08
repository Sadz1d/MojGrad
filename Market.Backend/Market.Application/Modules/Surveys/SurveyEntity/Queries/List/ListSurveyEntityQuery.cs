using MediatR;

namespace Market.Application.Modules.Surveys.Survey.Queries.List;

public sealed class ListSurveysQuery : BasePagedQuery<ListSurveysQueryDto>
{
    
    public string? Search { get; init; }
    public DateTime? ActiveOn { get; init; }

    public bool? OnlyActive { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}
