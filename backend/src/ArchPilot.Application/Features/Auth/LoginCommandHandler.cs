using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResult<AuthResponse>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IRepository<User> userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResult<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = (await _userRepository.FindAsync(u => u.Email == request.Email, cancellationToken)).FirstOrDefault();
        if (user == null)
            return ApiResult<AuthResponse>.Failure("Invalid email or password");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return ApiResult<AuthResponse>.Failure("Invalid email or password");

        if (!user.IsActive)
            return ApiResult<AuthResponse>.Failure("Account is deactivated");

        var token = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return ApiResult<AuthResponse>.SuccessResult(new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            User = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        });
    }
}
