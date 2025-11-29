using System.Text;

namespace IntroductionToWebAPIs.Services.Reports
{
    public class CsvReportGenerator : IReportGenerator
    {
        public byte[] GenerateReport()
        {
            string csvContent = "Товар,Кол-во,Цена\nНоутбук,5,1200\nМышь,10,25\nКлавиатура,7,45";
            return Encoding.UTF8.GetBytes(csvContent);
        }
    }
}
