using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class ChatCommand : IRequest<ApiResult<ChatResponse>>
{
    public Guid ProjectId { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? ConversationId { get; set; }
}

public class ChatResponse
{
    public string Answer { get; set; } = string.Empty;
    public Guid ConversationId { get; set; }
    public bool DocumentCreated { get; set; }
    public string? DocumentType { get; set; }
}
