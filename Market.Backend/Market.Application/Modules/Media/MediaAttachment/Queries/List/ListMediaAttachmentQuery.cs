using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Media.Queries.List;

public sealed class ListMediaAttachmentsQuery
    : BasePagedQuery<ListMediaAttachmentsQueryDto>
{
    public string? SearchMime { get; init; }   // Filtriranje po mime tipu (npr. image/jpeg)
    public int? UploaderId { get; init; }      // Filtriranje po korisniku
}
