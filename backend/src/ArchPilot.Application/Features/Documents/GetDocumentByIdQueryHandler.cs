using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, ApiResult<DocumentResponse>>
{
    private readonly IRepository<GeneratedDocument> _documentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetDocumentByIdQueryHandler(IRepository<GeneratedDocument> documentRepository, ICurrentUserService currentUserService)
    {
        _documentRepository = documentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<DocumentResponse>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<DocumentResponse>.Failure("User not authenticated");

        var document = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (document == null)
            return ApiResult<DocumentResponse>.Failure("Document not found");

        return ApiResult<DocumentResponse>.SuccessResult(new DocumentResponse
        {
            Id = document.Id,
            DocumentType = document.DocumentType.ToString(),
            Title = document.Title,
            Content = document.Content,
            Format = document.Format,
            Version = document.Version,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        });
    }
}
