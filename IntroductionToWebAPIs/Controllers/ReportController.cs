using IntroductionToWebAPIs.Enums;
using IntroductionToWebAPIs.Services.Reports;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        [HttpPost("{type}")]
        public IActionResult GetReport(string type)
        {
            var factory = new ReportFactory();

            ReportType reportType = type.ToLower() switch
            {
                "pdf" => ReportType.Pdf,
                "xlsx" => ReportType.xlsx,
                "csv" => ReportType.Csv,
                _ => throw new ArgumentException("Неверный тип отчёта")
            };

            var generator = factory.CreateReportGenerator(reportType);
            var fileBytes = generator.GenerateReport();

            string contentType = reportType switch
            {
                ReportType.Pdf => "application/pdf",
                ReportType.xlsx => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ReportType.Csv => "text/csv",
                _ => "application/octet-stream"
            };
             
            string fileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.{type.ToLower()}";
            return File(fileBytes, contentType, fileName);
        }
    }
}
