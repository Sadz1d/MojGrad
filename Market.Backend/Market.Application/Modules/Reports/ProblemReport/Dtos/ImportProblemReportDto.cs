using System.ComponentModel.DataAnnotations;

namespace Market.Application.Modules.Reports.ProblemReport.Dtos;

//public sealed class ImportProblemReportDto
//{
//    public string Title { get; set; } = default!;
//    public string? Description { get; set; }
//    public string? Location { get; set; }
//    public int CategoryId { get; set; }
//    public int StatusId { get; set; }
//    public int UserId { get; set; } = 1;
//}
public class ImportProblemReportDto
{
    [Required(ErrorMessage = "Title je obavezno polje")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title mora imati između 5 i 200 karaktera")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description je obavezno polje")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description mora imati između 10 i 2000 karaktera")]
    public string Description { get; set; }

    [StringLength(200, ErrorMessage = "Location ne može biti duži od 200 karaktera")]
    public string Location { get; set; }

    [Required(ErrorMessage = "CategoryId je obavezno polje")]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId mora biti pozitivan broj")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "StatusId je obavezno polje")]
    [Range(1, int.MaxValue, ErrorMessage = "StatusId mora biti pozitivan broj")]
    public int StatusId { get; set; }

    // može se postaviti automatski iz trenutnog korisnika
    public int? UserId { get; set; }
}