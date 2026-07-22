using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, ApiResult<List<ConversationResponse>>>
{
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetConversationsQueryHandler(IRepository<Conversation> conversationRepository, ICurrentUserService currentUserService)
    {
        _conversationRepository = conversationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<List<ConversationResponse>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<List<ConversationResponse>>.Failure("User not authenticated");

        var conversations = await _conversationRepository.FindAsync(c => c.ProjectId == request.ProjectId, cancellationToken);
        var response = conversations.OrderByDescending(c => c.UpdatedAt).Select(c => new ConversationResponse
        {
            Id = c.Id,
            Title = c.Title,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return ApiResult<List<ConversationResponse>>.SuccessResult(response);
    }
}
