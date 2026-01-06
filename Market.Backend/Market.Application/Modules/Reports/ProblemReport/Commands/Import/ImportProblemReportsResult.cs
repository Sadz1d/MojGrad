namespace Market.Application.Modules.Reports.ProblemReport.Commands.Import;

public class ImportProblemReportsResult
{
    public int TotalRecords { get; set; }
    public int Successful { get; set; }
    public int Failed { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<int> ImportedIds { get; set; } = new();

    [JsonIgnore]
    public bool IsSuccess => Failed == 0;
}