using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace funduq.Services
{
    public class PdfService
	{
       

        public byte[] GenerateInvoice(decimal totalAmount,string recepientName,string hotelName,string checkInDate,string checkOutDate,string roomName)
        {
            using (var ms = new MemoryStream())
            {
                // Initialize PDF writer and document
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Define the custom colors
                DeviceRgb mainColor = new DeviceRgb(65, 84, 241); // #4154f1
                DeviceRgb brighterBackgroundColor = new DeviceRgb(91, 109, 255); // Brighter shade for table background

                // Adding a title with the main color
                Paragraph titleParagraph = new Paragraph("Funduq Reservation Invoice")
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(mainColor);

                document.Add(titleParagraph);

                // Adding a greeting paragraph with the main color
                document.Add(new Paragraph($"Hello {recepientName}, this is your payment invoice")
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(mainColor)
                    .SetMarginTop(20));

                // Adding a table with 4 columns
                Table table = new Table(4);
                table.SetWidth(UnitValue.CreatePercentValue(100))
                     .SetMarginTop(20)
                     .SetTextAlignment(TextAlignment.CENTER);

                // Adding table headers with the main color
                table.AddHeaderCell(new Cell().Add(new Paragraph("Hotel")).SetFontSize(14).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontColor(mainColor));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Room")).SetFontSize(14).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontColor(mainColor));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Check-In Date")).SetFontSize(14).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontColor(mainColor));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Check-Out Date")).SetFontSize(14).SetBold().SetTextAlignment(TextAlignment.CENTER).SetFontColor(mainColor));

                // Adding table data with the brighter background color
                table.AddCell(new Cell().Add(new Paragraph(hotelName)).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph(roomName)).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph(checkInDate)).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph(checkOutDate)).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));

                document.Add(table);

                // Adding the total bill amount with the main color
                document.Add(new Paragraph($"Total Bill Amount: ${totalAmount}")
                    .SetFontSize(16)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontColor(mainColor)
                    .SetMarginTop(20));

                // Closing the document
                document.Close();

                // Return the PDF as a byte array
                return ms.ToArray();
            }
        }
    }
}

