using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.AI;

public class GetConversationsQuery : IRequest<ApiResult<List<ConversationResponse>>>
{
    public Guid ProjectId { get; set; }
}

public class ConversationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
