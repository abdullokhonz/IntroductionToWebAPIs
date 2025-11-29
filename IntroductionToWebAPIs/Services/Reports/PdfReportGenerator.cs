using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace IntroductionToWebAPIs.Services.Reports
{
    public class PdfReportGenerator : IReportGenerator
    {
        public byte[] GenerateReport()
        {
            using var ms = new MemoryStream();

            // Создаём writer/pdf/document
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            {
                var document = new Document(pdf);

                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Заголовок
                var title = new Paragraph("Отчёт по продажам")
                    .SetFont(boldFont)
                    .SetFontSize(20);
                document.Add(title);

                document.Add(new Paragraph($"Дата генерации: {DateTime.Now}")
                    .SetFont(regularFont)
                    .SetFontSize(10));

                // Таблица
                var table = new Table(3);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Товар").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Кол-во").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Цена").SetFont(boldFont)));

                table.AddCell(new Cell().Add(new Paragraph("Ноутбук").SetFont(regularFont)));
                table.AddCell(new Cell().Add(new Paragraph("5").SetFont(regularFont)));
                table.AddCell(new Cell().Add(new Paragraph("$1200").SetFont(regularFont)));

                table.AddCell(new Cell().Add(new Paragraph("Мышь").SetFont(regularFont)));
                table.AddCell(new Cell().Add(new Paragraph("10").SetFont(regularFont)));
                table.AddCell(new Cell().Add(new Paragraph("$25").SetFont(regularFont)));

                document.Add(table);

                document.Close();
            }

            return ms.ToArray();
        }
    }
}
