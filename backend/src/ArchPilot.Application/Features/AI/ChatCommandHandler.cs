using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using ArchPilot.Domain.Enums;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class ChatCommandHandler : IRequestHandler<ChatCommand, ApiResult<ChatResponse>>
{
    private readonly IRepository<Conversation> _conversationRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IAIService _aiService;
    private readonly IAIContextService _contextService;
    private readonly ICurrentUserService _currentUserService;

    public ChatCommandHandler(
        IRepository<Conversation> conversationRepository,
        IRepository<Message> messageRepository,
        IRepository<Project> projectRepository,
        IAIService aiService,
        IAIContextService contextService,
        ICurrentUserService currentUserService)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _projectRepository = projectRepository;
        _aiService = aiService;
        _contextService = contextService;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<ChatResponse>> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<ChatResponse>.Failure("User not authenticated");

        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null || project.UserId != currentUser.Id)
            return ApiResult<ChatResponse>.Failure("Project not found");

        Conversation conversation;
        if (request.ConversationId.HasValue)
        {
            var existing = await _conversationRepository.GetByIdAsync(request.ConversationId.Value, cancellationToken);
            if (existing == null || existing.ProjectId != request.ProjectId)
                return ApiResult<ChatResponse>.Failure("Conversation not found");
            conversation = existing;
        }
        else
        {
            conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                Title = request.Message.Length > 100 ? request.Message[..100] : request.Message,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _conversationRepository.AddAsync(conversation, cancellationToken);
        }

        var userMessage = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderType = SenderType.User,
            Content = request.Message,
            CreatedAt = DateTime.UtcNow
        };
        await _messageRepository.AddAsync(userMessage, cancellationToken);

        var projectContext = await _contextService.GetProjectContextAsync(request.ProjectId, cancellationToken);
        var agentType = DetermineAgentType(request.Message);
        var response = await _aiService.ChatAsync(request.Message, projectContext, agentType, currentUser.LanguagePreference, cancellationToken);

        var aiMessage = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderType = SenderType.AI,
            Content = response,
            TokenUsage = response.Length / 4,
            CreatedAt = DateTime.UtcNow
        };
        await _messageRepository.AddAsync(aiMessage, cancellationToken);

        conversation.UpdatedAt = DateTime.UtcNow;

        return ApiResult<ChatResponse>.SuccessResult(new ChatResponse
        {
            Answer = response,
            ConversationId = conversation.Id,
            DocumentCreated = false
        });
    }

    private AIAgentType DetermineAgentType(string message)
    {
        var lower = message.ToLowerInvariant();
        if (lower.Contains("srs") || lower.Contains("requirement")) return AIAgentType.RequirementsEngineer;
        if (lower.Contains("architecture") || lower.Contains("design")) return AIAgentType.SoftwareArchitect;
        if (lower.Contains("database") || lower.Contains("erd") || lower.Contains("schema")) return AIAgentType.DatabaseEngineer;
        if (lower.Contains("ui") || lower.Contains("ux") || lower.Contains("interface")) return AIAgentType.UXDesigner;
        if (lower.Contains("test") || lower.Contains("qa")) return AIAgentType.QAEngineer;
        if (lower.Contains("plan") || lower.Contains("sprint") || lower.Contains("timeline")) return AIAgentType.ProjectManager;
        if (lower.Contains("business") || lower.Contains("scope") || lower.Contains("analyze")) return AIAgentType.ProductManager;
        return AIAgentType.General;
    }
}
