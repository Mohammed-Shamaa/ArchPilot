using ArchPilot.Application.Common.Models;

namespace ArchPilot.Application.Common.Interfaces;

public interface ICurrentUserService
{
    CurrentUser? GetCurrentUser();
}
