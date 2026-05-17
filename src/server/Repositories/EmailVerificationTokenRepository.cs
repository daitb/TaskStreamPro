using Microsoft.EntityFrameworkCore;
using server.Data;

public class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
{
    private readonly AppDbContext _context;
    public EmailVerificationTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(EmailVerificationToken token)
    {
        _context.EmailVerificationTokens.Add(token);
    }

    public async Task<EmailVerificationToken?> GetByTokenAsync(string token)
    {
        return await _context.EmailVerificationTokens
        .FirstOrDefaultAsync(t => t.Token == token);
    }

    public void Remove(EmailVerificationToken token)
    {
        _context.EmailVerificationTokens.Remove(token);
    }
}
