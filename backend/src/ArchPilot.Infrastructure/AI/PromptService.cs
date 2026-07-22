using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Domain.Enums;
using ArchPilot.Domain.Entities;
using ArchPilot.Application.Common.Interfaces;

namespace ArchPilot.Infrastructure.AI;

public class PromptService : IPromptService
{
    private readonly IRepository<Prompt> _promptRepository;

    public PromptService(IRepository<Prompt> promptRepository)
    {
        _promptRepository = promptRepository;
    }

    public async Task<string> GetPromptAsync(AIAgentType agentType, CancellationToken cancellationToken = default)
    {
        var prompt = (await _promptRepository.FindAsync(
            p => p.AgentType == agentType && p.IsActive,
            cancellationToken)).FirstOrDefault();

        return prompt?.PromptContent ?? GetDefaultPrompt(agentType);
    }

    public async Task UpdatePromptAsync(AIAgentType agentType, string promptContent, CancellationToken cancellationToken = default)
    {
        var existing = (await _promptRepository.FindAsync(
            p => p.AgentType == agentType && p.IsActive,
            cancellationToken)).FirstOrDefault();

        if (existing != null)
        {
            existing.IsActive = false;
            _promptRepository.Update(existing);
        }

        var newPrompt = new Prompt
        {
            Id = Guid.NewGuid(),
            AgentType = agentType,
            PromptContent = promptContent,
            Version = (existing?.Version ?? 0) + 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _promptRepository.AddAsync(newPrompt, cancellationToken);
    }

    private string GetDefaultPrompt(AIAgentType agentType)
    {
        return agentType switch
        {
            AIAgentType.ProductManager => "You are a Senior Product Manager. Analyze business ideas, define scope, goals, and requirements. Ask clarifying questions when needed.",
            AIAgentType.RequirementsEngineer => "You are a Senior Requirements Engineer. Generate IEEE-style SRS documents with functional requirements, non-functional requirements, use cases, and user stories.",
            AIAgentType.SoftwareArchitect => "You are a Senior Software Architect. Design system architecture, recommend technology stacks, create UML diagrams, and API documentation.",
            AIAgentType.DatabaseEngineer => "You are a Senior Database Engineer. Design database schemas, ER diagrams, relationships, and optimization strategies.",
            AIAgentType.UXDesigner => "You are a Senior UX Designer. Plan user interfaces, user flows, components, and accessibility requirements.",
            AIAgentType.QAEngineer => "You are a Senior QA Engineer. Generate comprehensive test cases, testing strategies, and quality assurance plans.",
            AIAgentType.ProjectManager => "You are a Senior Project Manager. Create development roadmaps, sprint plans, task breakdowns, and timelines.",
            _ => "You are ArchPilot, an AI Software Engineering Assistant. Help users design, plan, and document software projects professionally."
        };
    }
}
