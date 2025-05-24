using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Identity
{
    /// <summary>
    /// Auth service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _jwtSettings = jwtOptions.Value;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
                throw new UnauthorizedAccessException("Permission denied!, no role found.");

            var token = GenerateJwtToken(user.Email, user.Id, $"{user.FirstName} {user.LastName}", roles.FirstOrDefault());

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
        }

        public async Task<AuthResponse> SignUpAsync(SignUpRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if(existingUser != null)
                throw new UnauthorizedAccessException("An account found with this email already!");

            var user = new ApplicationUser()
            {
                UserName = request.Email.Split('@')[0],
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.LastName,
            };

            await _userManager.CreateAsync(user, request.Password);

            var roles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user.Email, user.Id, $"{user.FirstName} {user.LastName}", roles.FirstOrDefault());

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
        }

        private string GenerateJwtToken(string email, int? userId, string name, string role)
        {
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Email, email ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, userId?.ToString() ?? string.Empty),
                    new Claim(ClaimTypes.GivenName, name ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
