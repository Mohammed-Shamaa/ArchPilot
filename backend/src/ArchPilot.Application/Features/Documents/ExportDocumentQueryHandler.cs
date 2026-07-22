using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class ExportDocumentQueryHandler : IRequestHandler<ExportDocumentQuery, ApiResult<byte[]>>
{
    private readonly IRepository<GeneratedDocument> _documentRepository;
    private readonly IDocumentExportService _exportService;
    private readonly ICurrentUserService _currentUserService;

    public ExportDocumentQueryHandler(
        IRepository<GeneratedDocument> documentRepository,
        IDocumentExportService exportService,
        ICurrentUserService currentUserService)
    {
        _documentRepository = documentRepository;
        _exportService = exportService;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<byte[]>> Handle(ExportDocumentQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<byte[]>.Failure("User not authenticated");

        var document = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (document == null)
            return ApiResult<byte[]>.Failure("Document not found");

        byte[] result = request.Format.ToLower() switch
        {
            "pdf" => await _exportService.ExportToPdfAsync(document.Content, document.Title, document.DocumentType),
            "docx" => await _exportService.ExportToDocxAsync(document.Content, document.Title, document.DocumentType),
            _ => await _exportService.ExportToPdfAsync(document.Content, document.Title, document.DocumentType)
        };

        return ApiResult<byte[]>.SuccessResult(result);
    }
}
