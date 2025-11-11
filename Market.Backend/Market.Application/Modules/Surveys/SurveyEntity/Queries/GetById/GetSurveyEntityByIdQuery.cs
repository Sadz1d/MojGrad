using MediatR;

namespace Market.Application.Modules.Surveys.Survey.Queries.GetById;

public sealed class GetSurveyByIdQuery : IRequest<GetSurveyByIdQueryDto>
{
    public int Id { get; init; }
}
