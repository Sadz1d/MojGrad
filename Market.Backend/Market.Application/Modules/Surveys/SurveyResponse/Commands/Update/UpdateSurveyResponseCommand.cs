using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Surveys.SurveyResponses.Commands.Update;

public sealed class UpdateSurveyResponseCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }        // dolazi iz rute: /api/survey-responses/{id}
    public string? ResponseText { get; set; }       // novi tekst odgovora (opcionalno)
}
