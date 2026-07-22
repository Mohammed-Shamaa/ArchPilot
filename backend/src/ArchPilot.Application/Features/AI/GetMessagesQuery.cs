using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class GetMessagesQuery : IRequest<ApiResult<List<MessageResponse>>>
{
    public Guid ConversationId { get; set; }
}

public class MessageResponse
{
    public Guid Id { get; set; }
    public string SenderType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
