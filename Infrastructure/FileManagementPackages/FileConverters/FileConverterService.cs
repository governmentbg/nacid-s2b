using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.Xml.Linq;
using fromHtml = iText.Html2pdf;

namespace Infrastructure.FileManagementPackages.FileConverters
{
    public class FileConverterService
    {
        public MemoryStream OpenXmlToPdf(MemoryStream documentStream)
        {
            return ConvertHTMLtoPDF(OpenXmlToHtml(documentStream));
        }

        public MemoryStream ConvertHTMLtoPDF(string html)
        {
            var htmlStream = new MemoryStream();
            var htmlWriter = new StreamWriter(htmlStream);
            htmlWriter.Write(html);
            htmlWriter.Flush();
            htmlStream.Position = 0;

            var pdfStream = new MemoryStream();
            var pdfWriter = new PdfWriter(pdfStream);
            var pdfDocument = new PdfDocument(pdfWriter);
            pdfDocument.SetDefaultPageSize(PageSize.A4);

            fromHtml.HtmlConverter.ConvertToPdf(htmlStream, pdfDocument);

            return pdfStream;
        }

        public string OpenXmlToHtml(MemoryStream documentStream)
        {
            var document = WordprocessingDocument.Open(documentStream, true);
            var settings = new OpenXmlPowerTools.HtmlConverterSettings();

            XElement html = OpenXmlPowerTools.HtmlConverter.ConvertToHtml(document, settings);
            return html.ToString();
        }
    }
}
