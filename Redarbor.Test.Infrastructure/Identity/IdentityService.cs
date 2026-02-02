using AutoMapper;
using Microsoft.Extensions.Logging;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Application.Interfaces;
using Redarbor.Test.Domain.Interfaces;

namespace Redarbor.Test.Infrastructure.Identity
{
    public class IdentityService : IAuthService
    {
        private readonly IEmployeeQueryRepository _queryRepository;
        private readonly IEmployeeRepository _repository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityService> _logger;
        private static readonly Dictionary<string, string> _refreshTokens = new();

        public IdentityService(
            IEmployeeQueryRepository queryRepository,
            IEmployeeRepository repository,
            IJwtTokenGenerator tokenGenerator,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<IdentityService> logger)
        {
            _queryRepository = queryRepository;
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(string username, string password)
        {
            _logger.LogInformation("Login attempt for username: {Username}", username);

            var employee = await _queryRepository.GetByUsernameAsync(username);

            if (employee == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (!ValidatePassword(password, employee.Password))
            {
                _logger.LogWarning("Invalid password for user: {Username}", username);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Get full employee entity to update last login
            var employeeEntity = await _repository.GetByIdAsync(employee.Id);
            if (employeeEntity != null)
            {
                employeeEntity.SetLastLogin();
                await _repository.UpdateAsync(employeeEntity);
                await _unitOfWork.CommitAsync();
            }

            // Generate tokens
            var token = _tokenGenerator.GenerateToken(employeeEntity!);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            // Store refresh token
            _refreshTokens[refreshToken] = employee.Id.ToString();

            _logger.LogInformation("Login successful for user: {Username}", username);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Employee = _mapper.Map<EmployeeDto>(employee)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt");

            if (!_refreshTokens.TryGetValue(refreshToken, out var employeeIdStr))
            {
                _logger.LogWarning("Invalid refresh token");
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var employeeId = int.Parse(employeeIdStr);
            var employeeEntity = await _repository.GetByIdAsync(employeeId);

            if (employeeEntity == null)
            {
                _logger.LogWarning("Employee not found for ID: {EmployeeId}", employeeId);
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var employee = await _queryRepository.GetByIdAsync(employeeId);

            // Generate new tokens
            var newToken = _tokenGenerator.GenerateToken(employee);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            // Remove old refresh token and store new one
            _refreshTokens.Remove(refreshToken);
            _refreshTokens[newRefreshToken] = employeeId.ToString();

            _logger.LogInformation("Token refreshed successfully for employee: {EmployeeId}", employeeId);

            return new AuthResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                Employee = _mapper.Map<EmployeeDto>(employee)
            };
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            var userId = _tokenGenerator.ValidateToken(token);
            return Task.FromResult(userId.HasValue);
        }

        private bool ValidatePassword(string inputPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }
    }
}
