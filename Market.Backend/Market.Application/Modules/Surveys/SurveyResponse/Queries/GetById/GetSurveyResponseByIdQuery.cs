using MediatR;

namespace Market.Application.Modules.Surveys.SurveyResponses.Queries.GetById;

public sealed class GetSurveyResponseByIdQuery : IRequest<GetSurveyResponseByIdQueryDto>
{
    public int Id { get; init; }
}
