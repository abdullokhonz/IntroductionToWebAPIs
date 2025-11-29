using ClosedXML.Excel;

namespace IntroductionToWebAPIs.Services.Reports
{
    public class ExcelReportGenerator : IReportGenerator
    {
        public byte[] GenerateReport()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Продажи");

            worksheet.Cell(1, 1).Value = "Товар";
            worksheet.Cell(1, 2).Value = "Кол-во";
            worksheet.Cell(1, 3).Value = "Цена";

            worksheet.Cell(2, 1).Value = "Ноутбук";
            worksheet.Cell(2, 2).Value = 5;
            worksheet.Cell(2, 3).Value = 1200;

            worksheet.Cell(3, 1).Value = "Мышь";
            worksheet.Cell(3, 2).Value = 10;
            worksheet.Cell(3, 3).Value = 25;

            worksheet.Cell(4, 1).Value = "Клавиатура";
            worksheet.Cell(4, 2).Value = 7;
            worksheet.Cell(4, 3).Value = 45;

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
