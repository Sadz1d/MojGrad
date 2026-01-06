// Market.Application/Modules/Reports/ProblemReport/Dtos/ImportProblemReportDtoMap.cs
using CsvHelper.Configuration;
using Market.Application.Modules.Reports.ProblemReport.Dtos;

namespace Market.Application.Modules.Reports.ProblemReport.Dtos
{
    public sealed class ImportProblemReportDtoMap : ClassMap<ImportProblemReportDto>
    {
        public ImportProblemReportDtoMap()
        {
            // Mapiranje za različite nazive kolona
            Map(m => m.Title).Name("title", "Title", "Naslov");
            Map(m => m.Description).Name("description", "Description", "Opis");
            Map(m => m.Location).Name("location", "Location", "Lokacija").Optional();
            Map(m => m.CategoryId).Name("categoryId", "CategoryId", "KategorijaId");
            Map(m => m.StatusId).Name("statusId", "StatusId", "StatusId");
            Map(m => m.UserId).Name("userId", "UserId", "KorisnikId").Optional();
        }
    }
}