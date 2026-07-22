using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Interfaces;

public interface IDocumentExportService
{
    Task<byte[]> ExportToPdfAsync(string content, string title, DocumentType documentType);
    Task<byte[]> ExportToDocxAsync(string content, string title, DocumentType documentType);
}
