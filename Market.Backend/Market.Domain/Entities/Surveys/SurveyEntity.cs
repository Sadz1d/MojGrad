using Market.Domain.Common;

namespace Market.Domain.Entities.Surveys;

public class SurveyEntity : BaseEntity
{
    public string Question { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public IReadOnlyCollection<SurveyResponseEntity> Responses { get; private set; } = new List<SurveyResponseEntity>();

    public static class Constraints
    {
        public const int QuestionMaxLength = 500;
    }
}
