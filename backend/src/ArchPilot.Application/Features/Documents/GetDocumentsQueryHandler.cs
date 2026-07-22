using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, ApiResult<List<DocumentResponse>>>
{
    private readonly IRepository<GeneratedDocument> _documentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetDocumentsQueryHandler(IRepository<GeneratedDocument> documentRepository, ICurrentUserService currentUserService)
    {
        _documentRepository = documentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<List<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<List<DocumentResponse>>.Failure("User not authenticated");

        var documents = await _documentRepository.FindAsync(d => d.ProjectId == request.ProjectId, cancellationToken);
        var response = documents.OrderByDescending(d => d.CreatedAt).Select(d => new DocumentResponse
        {
            Id = d.Id,
            DocumentType = d.DocumentType.ToString(),
            Title = d.Title,
            Content = d.Content,
            Format = d.Format,
            Version = d.Version,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        }).ToList();

        return ApiResult<List<DocumentResponse>>.SuccessResult(response);
    }
}
