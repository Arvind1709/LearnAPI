using DinkToPdf.Contracts;
using iText.Kernel.Pdf;
using LearnAPI.AppDbContext;
using LearnAPI.ModelDTO;
//using MimeKit;
//using System.Net.Mail;

using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaInvoiceController : ControllerBase
    {
        private readonly BookNookDbContext _context;
        private readonly IConverter _converter;
        public ProformaInvoiceController(BookNookDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }


        [HttpGet("GenerateProforma/{orderId}")]
        public async Task<IActionResult> GenerateProformaInvoice(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Order not found!");
            }

            var proformaInvoice = new ProformaInvoiceDto
            {
                OrderId = order.Id,
                BuyerName = "Customer Name", // (You can fetch from User table if needed)
                InvoiceDate = DateTime.UtcNow,
                Items = order.OrderItems.Select(oi => new ProformaItemDto
                {
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    PricePerUnit = oi.Price
                }).ToList(),
                TotalAmount = order.TotalAmount
            };

            return Ok(proformaInvoice);
        }

        [HttpGet("DownloadPI123/{orderId}")]
        public async Task<IActionResult> DownloadProformaInvoice(int orderId)
        {
            //var proformaData = await GenerateProformaInvoice(orderId); // Assume this fetches the same JSON structure you shared
            //if (proformaData == null)
            //   return NotFound();
            //var proformaInvoice1 = new ProformaInvoiceDto;

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Order not found!");
            }

            var proformaInvoice = new ProformaInvoiceDto
            {
                OrderId = order.Id,
                BuyerName = "Customer Name", // (You can fetch from User table if needed)
                InvoiceDate = DateTime.UtcNow,
                Items = order.OrderItems.Select(oi => new ProformaItemDto
                {
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    PricePerUnit = oi.Price
                }).ToList(),
                TotalAmount = order.TotalAmount
            };


            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var doc = new iText.Layout.Document(pdf);

            doc.Add(new iText.Layout.Element.Paragraph("Proforma Invoice").SetFontSize(18).SetBold());
            doc.Add(new iText.Layout.Element.Paragraph($"Order ID: {proformaInvoice.OrderId}"));
            doc.Add(new iText.Layout.Element.Paragraph($"Buyer Name: {proformaInvoice.BuyerName}"));
            doc.Add(new iText.Layout.Element.Paragraph($"Invoice Date: {proformaInvoice.InvoiceDate:dd-MM-yyyy}"));
            doc.Add(new iText.Layout.Element.Paragraph(" ")); // Space

            // Table for items
            var table = new iText.Layout.Element.Table(4);
            table.AddHeaderCell("Book Title");
            table.AddHeaderCell("Quantity");
            table.AddHeaderCell("Price/Unit");
            table.AddHeaderCell("Total");

            foreach (var item in proformaInvoice.Items)
            {
                table.AddCell(item.BookTitle);
                table.AddCell(item.Quantity.ToString());
                table.AddCell(item.PricePerUnit.ToString("C"));
                table.AddCell(item.TotalPrice.ToString("C"));
            }

            doc.Add(table);
            doc.Add(new iText.Layout.Element.Paragraph($"Total Amount: {proformaInvoice.TotalAmount:C}").SetBold());
            doc.Close();

            return File(stream.ToArray(), "application/pdf", $"ProformaInvoice_Order_{orderId}.pdf");
        }


        [HttpGet("DownloadPI/{orderId}")]
        public async Task<IActionResult> DownloadProformaInvoice1(int orderId)
        {
            // Fetch order details from the database (dummy data example)
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Generate dynamic table rows for the PDF
            string itemsHtml = "";
            foreach (var item in order.OrderItems)
            {
                itemsHtml += $@"
            <tr>
                <td>{item.Book.Title}</td>
                <td>{item.Quantity}</td>
                <td>{item.Price}</td>
                <td>{item.Quantity * item.Price}</td>
            </tr>";
            }

            // Build the HTML content
            string htmlContent = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; }}
                            table {{ width: 100%; border-collapse: collapse; }}
                            th, td {{ border: 1px solid #ddd; padding: 8px; }}
                            th {{ background-color: #f2f2f2; }}
                            h2 {{ text-align: center; }}
                        </style>
                    </head>
                    <body>
                        <h2>Proforma Invoice</h2>
                        <p><strong>Buyer Name:</strong> Customer Name</p>
                        <p><strong>Invoice Date:</strong> {DateTime.UtcNow:dd-MM-yyyy}</p>

                        <table>
                            <tr>
                                <th>Book Title</th>
                                <th>Quantity</th>
                                <th>Price/Unit</th>
                                <th>Total</th>
                            </tr>
                            {itemsHtml}
                        </table>

                        <h3>Total Amount: ₹{order.TotalAmount}</h3>
                    </body>
                    </html>";

            // Configure DinkToPdf
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                DocumentTitle = "Proforma Invoice"
            };

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdfDoc = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            // Convert HTML to PDF
            var pdf = _converter.Convert(pdfDoc);
            byte[] pdfBytes = pdf; // assuming pdf is a byte[] or MemoryStream

            string toEmail = "customer@example.com";
            string subject = "Your Proforma Invoice";
            string body = "<p>Dear Customer,</p><p>Please find attached your proforma invoice.</p>";

            await SendInvoiceEmailAsync(toEmail, subject, body, pdfBytes);

            // Return the PDF file as a response
            return File(pdf, "application/pdf", $"ProformaInvoice_Order_{orderId}.pdf");
        }

        public async Task SendInvoiceEmailAsync(string toEmail, string subject, string body, byte[] pdfBytes)
        {
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("The Book Nook", "your_email@example.com"));
            //message.To.Add(new MailboxAddress("", toEmail));
            //message.Subject = subject;

            //// Create body with attachment
            //var builder = new BodyBuilder
            //{
            //    HtmlBody = body
            //};

            //// Attach the PDF file
            //builder.Attachments.Add("ProformaInvoice.pdf", pdfBytes, new ContentType("application", "pdf"));

            //message.Body = builder.ToMessageBody();

            //using (var client = new SmtpClient())
            //{
            //    // Replace with your SMTP details
            //    await client.ConnectAsync("smtp.yourmailserver.com", 587, false);
            //    await client.AuthenticateAsync("your_email@example.com", "your_email_password");
            //    await client.SendAsync(message);
            //    await client.DisconnectAsync(true);
            //}

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "your_email@gmail.com"));
            message.To.Add(new MailboxAddress("", "receiver@example.com"));
            message.Subject = "Proforma Invoice";
            var builder = new BodyBuilder
            {
                HtmlBody = "<p>Please find attached the proforma invoice.</p>"
            };

            // Attach the PDF
            builder.Attachments.Add("ProformaInvoice.pdf", pdfBytes, new ContentType("application", "pdf"));
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("your_email@gmail.com", "your_app_password");  // Use App Password
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}
