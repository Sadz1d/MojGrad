using MediatR;

namespace Market.Application.Modules.Surveys.Survey.Queries.List;

public sealed class ListSurveysQuery : BasePagedQuery<ListSurveysQueryDto>
{
    public string? Search { get; init; }       // pretraga po pitanju (Question)
    public DateTime? ActiveOn { get; init; }   // prikaz samo aktivnih anketa na određeni datum
}
