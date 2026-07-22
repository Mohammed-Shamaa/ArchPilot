using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Auth;

public class LoginCommand : IRequest<ApiResult<AuthResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
