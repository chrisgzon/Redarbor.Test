using Redarbor.Test.Application.DTOs;

namespace Redarbor.Test.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(string username, string password);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}
