using Redarbor.Test.Domain.Entities;

namespace Redarbor.Test.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Employee employee);
        string GenerateRefreshToken();
        int? ValidateToken(string token);
    }
}
