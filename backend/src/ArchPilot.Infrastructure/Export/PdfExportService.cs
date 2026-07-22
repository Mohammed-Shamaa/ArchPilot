using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArchPilot.Infrastructure.Export;

public class PdfExportService : IDocumentExportService
{
    public PdfExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Task<byte[]> ExportToPdfAsync(string content, string title, DocumentType documentType)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);
                page.Header().Text($"ArchPilot - {documentType}").FontSize(10).FontColor(Colors.Grey.Medium);
                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Item().Text(title).FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten4);
                    col.Item().PaddingTop(10).Text(content).FontSize(11);
                });
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        });

        using var stream = new MemoryStream();
        document.GeneratePdf(stream);
        return Task.FromResult(stream.ToArray());
    }

    public Task<byte[]> ExportToDocxAsync(string content, string title, DocumentType documentType)
    {
        return ExportToPdfAsync(content, title, documentType);
    }
}
