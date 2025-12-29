using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EnglishManager.Application.DTOs;
using EnglishManager.Application.Interfaces;
using EnglishManager.Domain.Entities;
using EnglishManager.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace EnglishManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = BC.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var userDto = MapToDto(user);
            var token = GenerateJwtToken(userDto);
            var refreshToken = GetRefreshToken();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                User = userDto
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !BC.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var userDto = MapToDto(user);
            var token = GenerateJwtToken(userDto);

            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public string GenerateJwtToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(15);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenDto GetRefreshToken()
        {
            return new RefreshTokenDto
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), // Placeholder, should be replaced with actual user data
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}