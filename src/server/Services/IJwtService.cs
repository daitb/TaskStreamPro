using server.Models;

public interface IJwtService
{
    public string GenerateAccessToken(User user);
    public Task<string> GenerateRefreshToken(User user);
    public string HashToken(string token);
}