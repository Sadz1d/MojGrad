using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Surveys.Survey.Commands.Update;

public sealed class UpdateSurveyCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }      // dolazi iz rute /api/surveys/{id}
    public string? Question { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
