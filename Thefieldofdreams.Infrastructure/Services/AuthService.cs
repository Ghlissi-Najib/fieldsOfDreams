using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Application.Security;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<AppUser> userManager, IJwtService jwtService, IConfiguration config, IEmailService emailService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _config = config;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new InvalidOperationException("Passwords do not match.");

            var allowedRoles = Enum.GetNames<UserRole>();
            if (!allowedRoles.Contains(dto.Role))
                throw new InvalidOperationException($"Invalid role. Allowed roles: {string.Join(", ", allowedRoles)}");

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                throw new InvalidOperationException("User with this email already exists.");

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                IsActive = true,
                Role = Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var parsedRole)
                    ? parsedRole
                    : UserRole.Passenger,
                MerchantId = dto.MerchantId,
                PartnerId = dto.PartnerId
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return await GenerateAuthResponseAsync(user);
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new UnauthorizedAccessException("Invalid credentials.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var userId = _jwtService.GetUserIdFromExpiredToken(dto.AccessToken)
                ?? throw new UnauthorizedAccessException("Invalid access token.");

            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new UnauthorizedAccessException("User not found.");

            if (user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            return await GenerateAuthResponseAsync(user);
        }

        public async Task RevokeTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            // Always succeed to prevent email enumeration attacks
            if (user is null) return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(dto.Email);

            var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:4200";
            var resetLink = $"{frontendUrl}/reset-password?email={encodedEmail}&token={encodedToken}";

            await _emailService.SendPasswordResetEmailAsync(dto.Email, resetLink);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new InvalidOperationException("Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new KeyNotFoundException("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Password reset failed: {errors}");
            }
        }

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email!, $"{user.FirstName} {user.LastName}", roles);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenExpiryDays = int.Parse(
                _config["JwtSettings:RefreshTokenExpiryDays"] ?? "7");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
            await _userManager.UpdateAsync(user);

            var jwtExpiresInMinutes = int.Parse(
                _config["JwtSettings:ExpiresInMinutes"] ?? "60");

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(jwtExpiresInMinutes),
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                Role = user.Role.ToString(),
                MerchantId = user.MerchantId,
                PartnerId = user.PartnerId,
                Roles = roles
            };
        }
    }

}
