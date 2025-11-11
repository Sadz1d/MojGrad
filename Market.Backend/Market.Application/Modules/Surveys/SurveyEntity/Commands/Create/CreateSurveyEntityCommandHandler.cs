using MediatR;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Surveys;

namespace Market.Application.Modules.Surveys.Survey.Commands.Create;

public sealed class CreateSurveyCommandHandler
    : IRequestHandler<CreateSurveyCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateSurveyCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateSurveyCommand request, CancellationToken ct)
    {
        var question = request.Question.Trim();
        if (string.IsNullOrWhiteSpace(question))
            throw new ArgumentException("Question is required.");


        if (request.EndDate <= request.StartDate)
            throw new ArgumentException("Question is required.");


        var entity = new SurveyEntity
        {
            Question = question,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _ctx.Surveys.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}
