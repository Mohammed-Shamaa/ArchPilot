using ArchPilot.Application.Common.Models;
using ArchPilot.Application.Features.AI;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AIController : ControllerBase
{
    private readonly IMediator _mediator;

    public AIController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<ApiResult<ChatResponse>>> Chat([FromBody] ChatCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("projects/{projectId:guid}/conversations")]
    public async Task<ActionResult<ApiResult<List<ConversationResponse>>>> GetConversations(Guid projectId)
    {
        var result = await _mediator.Send(new GetConversationsQuery { ProjectId = projectId });
        return Ok(result);
    }

    [HttpGet("conversations/{conversationId:guid}/messages")]
    public async Task<ActionResult<ApiResult<List<MessageResponse>>>> GetMessages(Guid conversationId)
    {
        var result = await _mediator.Send(new GetMessagesQuery { ConversationId = conversationId });
        return Ok(result);
    }
}