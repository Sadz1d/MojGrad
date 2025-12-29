using MediatR;

namespace Market.Application.Modules.Surveys.Commands.Status.Enable;

public sealed class EnableSurveyCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
