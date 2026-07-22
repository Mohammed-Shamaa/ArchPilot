using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ApiResult<List<MessageResponse>>>
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMessagesQueryHandler(
        IRepository<Message> messageRepository,
        IRepository<Conversation> conversationRepository,
        ICurrentUserService currentUserService)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<List<MessageResponse>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<List<MessageResponse>>.Failure("User not authenticated");

        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
            return ApiResult<List<MessageResponse>>.Failure("Conversation not found");

        var messages = await _messageRepository.FindAsync(m => m.ConversationId == request.ConversationId, cancellationToken);
        var response = messages.OrderBy(m => m.CreatedAt).Select(m => new MessageResponse
        {
            Id = m.Id,
            SenderType = m.SenderType.ToString(),
            Content = m.Content,
            CreatedAt = m.CreatedAt
        }).ToList();

        return ApiResult<List<MessageResponse>>.SuccessResult(response);
    }
}
