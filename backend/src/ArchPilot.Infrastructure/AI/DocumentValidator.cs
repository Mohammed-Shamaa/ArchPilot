using System.Text.RegularExpressions;
using ArchPilot.Application.Common.Interfaces;

namespace ArchPilot.Infrastructure.AI;

public class DocumentValidator : IDocumentValidator
{
    public bool ValidateSRS(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;
        var requiredSections = new[] { "Introduction", "Overall Description", "Functional Requirements", "Non-Functional Requirements" };
        return requiredSections.All(s => content.Contains(s, StringComparison.OrdinalIgnoreCase));
    }

    public bool ValidateERD(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;
        return content.Contains("erDiagram") || content.Contains("graph") || content.Contains("class");
    }

    public bool ValidateUML(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;
        return content.Contains("graph") || content.Contains("sequenceDiagram") || content.Contains("classDiagram") || content.Contains("flowchart");
    }

    public bool ValidateAPI(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;
        return content.Contains("GET") || content.Contains("POST") || content.Contains("PUT") || content.Contains("DELETE");
    }
}
