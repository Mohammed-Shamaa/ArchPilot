using ArchPilot.Application.Common.Models;
using ArchPilot.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResult<AuthResponse>>> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResult<AuthResponse>>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
            return Unauthorized(result);
        return Ok(result);
    }
}