using server.Models;

public class EmailVerificationToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;

    public virtual User User { get; set; } = null!;
}