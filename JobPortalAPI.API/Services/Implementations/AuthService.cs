using JobPortalAPI.API.Data;
using JobPortalAPI.API.DTOs.Auth;
using JobPortalAPI.API.Exceptions;
using JobPortalAPI.API.Helpers;
using JobPortalAPI.API.Models;
using JobPortalAPI.API.Repositories.Interfaces;
using JobPortalAPI.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var exists = await _userRepository.GetByEmailAsync(registerDto.Email);

        if (exists != null)
        {
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

        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

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
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null)
            throw new NotFoundException("User not found.");

        if (!PasswordHasher.Verify(loginDto.Password, user.PasswordHash))
            throw new BadRequestException("Invalid password.");


        // Generate JWT token
        var token = _jwtHelper.GenerateToken(user);

        // Return response
        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        };
    }

}