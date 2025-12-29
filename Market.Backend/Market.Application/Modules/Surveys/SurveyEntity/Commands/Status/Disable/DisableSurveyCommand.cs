using MediatR;

namespace Market.Application.Modules.Surveys.Commands.Status.Disable;

public sealed class DisableSurveyCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
