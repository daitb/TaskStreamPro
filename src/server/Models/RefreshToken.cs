using server.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public bool isRevoked { get; set; } = false;

    public User User { get; set; } = null!;
}