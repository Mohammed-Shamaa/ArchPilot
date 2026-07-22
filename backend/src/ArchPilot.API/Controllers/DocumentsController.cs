using ArchPilot.Application.Common.Models;
using ArchPilot.Application.Features.Documents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<ApiResult<List<DocumentResponse>>>> GetByProject(Guid projectId)
    {
        var result = await _mediator.Send(new GetDocumentsQuery { ProjectId = projectId });
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResult<DocumentResponse>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDocumentByIdQuery { Id = id });
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> Export(Guid id, [FromQuery] string format = "pdf")
    {
        var result = await _mediator.Send(new ExportDocumentQuery { Id = id, Format = format });
        if (!result.Success)
            return BadRequest(result);

        var contentType = format.ToLower() switch
        {
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/pdf"
        };

        var fileName = $"archpilot-document.{format}";
        return File(result.Data!, contentType, fileName);
    }
}