using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Surveys;

public class SurveyResponseEntity : BaseEntity
{
    public int SurveyId { get; set; }
    public int UserId { get; set; }
    public string ResponseText { get; set; } = default!;

    public SurveyEntity Survey { get; set; } = default!;
    public MarketUserEntity User { get; set; } = default!;

    public static class Constraints
    {
        public const int ResponseMaxLength = 1000;
    }
}
