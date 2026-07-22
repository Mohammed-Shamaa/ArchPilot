using ArchPilot.Application.Common.Models;
using ArchPilot.Application.Features.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResult<ProjectResponse>>> Create([FromBody] CreateProjectCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success)
            return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<List<ProjectResponse>>>> GetAll()
    {
        var result = await _mediator.Send(new GetProjectsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResult<ProjectResponse>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery { Id = id });
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }
}