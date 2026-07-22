using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace ArchPilot.Infrastructure.AI;

public class GrokAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IPromptService _promptService;

    public GrokAIService(HttpClient httpClient, IConfiguration configuration, IPromptService promptService)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _promptService = promptService;
    }

    public async Task<string> ChatAsync(string message, string context, AIAgentType agentType, string language = "en", CancellationToken cancellationToken = default)
    {
        var systemPrompt = await _promptService.GetPromptAsync(agentType, cancellationToken);
        var languageInstruction = language switch
        {
            "ar" => "\n\nIMPORTANT: Respond in Arabic language. Use professional technical Arabic.",
            _ => ""
        };

        var requestBody = new
        {
            model = "grok-3",
            messages = new object[]
            {
                new { role = "system", content = systemPrompt + languageInstruction },
                new { role = "user", content = $"Project Context:\n{context}\n\nUser Message:\n{message}" }
            },
            max_tokens = 4096,
            temperature = 0.7
        };

        var response = await _httpClient.PostAsJsonAsync(
            _configuration["AI:ApiUrl"] + "/chat/completions",
            requestBody,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        return result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }

    public async Task<string> GenerateDocumentAsync(string projectDescription, DocumentType documentType, string context, string language = "en", CancellationToken cancellationToken = default)
    {
        var agentType = documentType switch
        {
            DocumentType.SRS => AIAgentType.RequirementsEngineer,
            DocumentType.Architecture => AIAgentType.SoftwareArchitect,
            DocumentType.ERD => AIAgentType.DatabaseEngineer,
            DocumentType.UML => AIAgentType.SoftwareArchitect,
            DocumentType.APIDocumentation => AIAgentType.SoftwareArchitect,
            DocumentType.TestCases => AIAgentType.QAEngineer,
            DocumentType.RiskAnalysis => AIAgentType.ProductManager,
            DocumentType.Timeline => AIAgentType.ProjectManager,
            DocumentType.UIUXPlan => AIAgentType.UXDesigner,
            _ => AIAgentType.General
        };

        var systemPrompt = await _promptService.GetPromptAsync(agentType, cancellationToken);
        var languageInstruction = language switch
        {
            "ar" => "\n\nIMPORTANT: Generate the document in Arabic language with proper formatting.",
            _ => ""
        };

        var documentTypeInstruction = documentType switch
        {
            DocumentType.SRS => "\n\nGenerate a complete IEEE-style Software Requirements Specification document.",
            DocumentType.Architecture => "\n\nGenerate a complete System Architecture Design document with technology recommendations.",
            DocumentType.ERD => "\n\nGenerate a complete Entity Relationship Diagram in Mermaid syntax.",
            DocumentType.UML => "\n\nGenerate complete UML diagrams in Mermaid syntax including Use Case, Class, Sequence, and Activity diagrams.",
            DocumentType.APIDocumentation => "\n\nGenerate complete API documentation with endpoints, methods, request/response formats.",
            DocumentType.TestCases => "\n\nGenerate a comprehensive QA test case document with test cases, steps, and expected results.",
            DocumentType.RiskAnalysis => "\n\nGenerate a complete Risk Analysis document with technical, security, and business risks.",
            DocumentType.Timeline => "\n\nGenerate a complete development timeline with phases, sprints, and milestones.",
            DocumentType.UIUXPlan => "\n\nGenerate a complete UI/UX Design Plan with pages, components, and user flows.",
            _ => ""
        };

        var requestBody = new
        {
            model = "grok-3",
            messages = new object[]
            {
                new { role = "system", content = systemPrompt + languageInstruction + documentTypeInstruction },
                new { role = "user", content = $"Project Description:\n{projectDescription}\n\nAdditional Context:\n{context}" }
            },
            max_tokens = 8192,
            temperature = 0.7
        };

        var response = await _httpClient.PostAsJsonAsync(
            _configuration["AI:ApiUrl"] + "/chat/completions",
            requestBody,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        return result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }
}
