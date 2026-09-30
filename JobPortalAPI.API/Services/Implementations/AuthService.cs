using JobPortalAPI.API.DTOs.Auth;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Helpers;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;

namespace JobPortalAPI.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, JwtHelper jwtHelper, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        _logger.LogInformation(
            "Register request received for {Email}",
            registerDto.Email);

        var exists = await _userRepository.GetByEmailAsync(registerDto.Email);

        if (exists != null)
        {
            _logger.LogWarning(
                "Registration failed: Email {Email} already exists",
                registerDto.Email);
            throw new BadRequestException("Email already exists.");
        }

        // Create user
        var user = new User
        {
            FullName = registerDto.FullName,
            Email = registerDto.Email,
            PasswordHash = PasswordHasher.Hash(registerDto.Password),
            Role = registerDto.Role
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

        _logger.LogInformation(
            "User registered successfully: {Email} with role {Role}",
            user.Email,
            user.Role);

        // Return response
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }


    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation(
            "Login request received for {Email}",
            loginDto.Email);

        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null)
        {
            _logger.LogWarning(
                "Login failed: User with email {Email} not found",
                loginDto.Email);

            throw new BadRequestException("User not found.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning(
                "Login failed: User with email {Email} is inactive",
                loginDto.Email);

            throw new ForbiddenException("Your account is inactive.");
        }

        if (!PasswordHasher.Verify(loginDto.Password, user.PasswordHash))
        {
            _logger.LogWarning(
                "Login failed: Invalid password for user {Email}",
                loginDto.Email);

            throw new BadRequestException("Invalid password.");
        }

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

        _logger.LogInformation(
            "User logged in successfully: {Email} with role {Role}",
            user.Email, user.Role);

        // Return response
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }

}