using server.Models;

public interface IJwtService
{
    public string GenerateToken(User user);
}