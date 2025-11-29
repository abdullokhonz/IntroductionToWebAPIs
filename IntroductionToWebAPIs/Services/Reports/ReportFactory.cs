using IntroductionToWebAPIs.Enums;

namespace IntroductionToWebAPIs.Services.Reports
{
    public class ReportFactory : IReportFactory
    {
        public IReportGenerator CreateReportGenerator(ReportType type)
        {
            return type switch
            {
                ReportType.Pdf => new PdfReportGenerator(),
                ReportType.xlsx => new ExcelReportGenerator(),
                ReportType.Csv => new CsvReportGenerator(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
