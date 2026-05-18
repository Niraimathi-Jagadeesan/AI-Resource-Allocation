using ResourceAllocation.API.Models;

namespace ResourceAllocation.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user);
    }
}