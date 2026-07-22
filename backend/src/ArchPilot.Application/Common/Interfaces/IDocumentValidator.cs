using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Interfaces;

public interface IDocumentValidator
{
    bool ValidateSRS(string content);
    bool ValidateERD(string content);
    bool ValidateUML(string content);
    bool ValidateAPI(string content);
}
