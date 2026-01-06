using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Market.API.Models.Requests
{
    public class ImportProblemReportsRequest
    {
        [Required]
        public IFormFile File { get; set; }

        public bool SkipFirstRow { get; set; } = true;
        public bool DryRun { get; set; } = false;
    }
}