using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using ArchPilot.Domain.Enums;
using MediatR;

namespace ArchPilot.Application.Features.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResult<AuthResponse>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        IRepository<User> userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResult<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userRepository.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (existingEmail)
            return ApiResult<AuthResponse>.Failure("Email already registered");

        var existingUsername = await _userRepository.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (existingUsername)
            return ApiResult<AuthResponse>.Failure("Username already taken");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(user, cancellationToken);

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
