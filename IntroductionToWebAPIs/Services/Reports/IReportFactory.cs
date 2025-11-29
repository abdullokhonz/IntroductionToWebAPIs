using IntroductionToWebAPIs.Enums;

namespace IntroductionToWebAPIs.Services.Reports
{
    public interface IReportFactory
    {
        IReportGenerator CreateReportGenerator(ReportType type);
    }
}
