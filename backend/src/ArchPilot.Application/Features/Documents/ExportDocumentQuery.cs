using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class ExportDocumentQuery : IRequest<ApiResult<byte[]>>
{
    public Guid Id { get; set; }
    public string Format { get; set; } = "pdf";
}
